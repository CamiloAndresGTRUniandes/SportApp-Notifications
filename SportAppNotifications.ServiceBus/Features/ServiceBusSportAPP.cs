namespace SportAppNotifications.ServiceBus.Features ;
using Application.Constants;
using Application.Contracts;
using Application.Settings;
using Azure.Messaging.ServiceBus;
using Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Signalrs;

    public class ServiceBusSportAPP : IServiceBusSportAPP
    {
        private readonly IHubContext<ChatHubSignal> _hubContext;

        private readonly ServiceMqSettings _serviceMqSettings;

        public ServiceBusSportAPP(IOptions<ServiceMqSettings> serviceMqSettings,
            IHubContext<ChatHubSignal> hubContext
            )
        {
            _hubContext = hubContext;
            _serviceMqSettings = serviceMqSettings.Value;
        }

        public async Task ListenerServicesSportApp()
        {
            var lstQueues = QueuesSportApp.Queues;
            var client = new ServiceBusClient(_serviceMqSettings.Endpoint);
            List<ServiceBusProcessor> processors = new();


            try
            {
                foreach (var queue in lstQueues)
                {
                    var processor = client.CreateProcessor(queue);
                    processor.ProcessMessageAsync += MessageHandler;
                    processor.ProcessErrorAsync += ErrorHandler;
                    await processor.StartProcessingAsync();
                    processors.Add(processor);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            Console.WriteLine(body);
            Console.WriteLine(args.EntityPath);
            if (args.EntityPath == QueuesSportApp.UserRecommendationsQueue)
            {
                await SendEventRecommendation(body);
            }
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }


        private async Task SendEventRecommendation(string body)
        {
            var recommendation = JsonConvert.DeserializeObject<EventsUsersEventBus>(body);
            await _hubContext.Clients.All.SendAsync($"ReceiveMessage{recommendation.UserId}", JsonConvert.SerializeObject(body));
        }
    }
