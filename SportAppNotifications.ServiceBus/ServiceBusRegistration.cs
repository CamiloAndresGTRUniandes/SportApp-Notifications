namespace SportAppNotifications.ServiceBus ;
using Application.Contracts;
using Application.Settings;
using Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Signalrs;

    public static class ServiceBusRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IServiceBusSportAPP, ServiceBusSportAPP>(sp =>
            {
                var optionsFactory = sp.GetService<IOptions<ServiceMqSettings>>();
                var signalScope = sp.GetService<IHubContext<ChatHubSignal>>();
                return new ServiceBusSportAPP(optionsFactory, signalScope);
            });


            services.AddSignalR();

            return services;
        }
    }

/*
 
 public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
   {
       //services.AddTransient<IRequestHandler<CreateEventoMasivoCommand, bool>, EventoMasivoCommandHandler>();

       //MediatR Mediator
       services.AddMediatR(Assembly.GetExecutingAssembly());

       //Domain Bus
       services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
       {
           var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
           var optionsFactory = sp.GetService<IOptions<RabbitMQSettings>>();
           return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory, optionsFactory);
       });
 */
