using Microsoft.AspNetCore.Cors.Infrastructure;
using SportAppNotifications.Application.Contracts;
using SportAppNotifications.Application.Settings;
using SportAppNotifications.ServiceBus;
using SportAppNotifications.Signalrs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<ServiceMqSettings>(builder.Configuration.GetSection("ServiceMqSettings"));

/*
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
                .WithOrigins(
                    "http://127.0.0.1:7070",
                    "http://locahost:7070",
                    "http://127.0.0.1:3030",
                    "http://locahost:3030",
                    "https://lively-pond-069dcf00f.5.azurestaticapps.net"
                )
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .SetIsOriginAllowed(host => true)
                .AllowAnyHeader()
            );

        options.AddPolicy("signalr",
            builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(hostName => true));
    });
    */
    builder.Services.AddCors(delegate(CorsOptions options)
    {
        options.AddPolicy("CorsPolicy", delegate(CorsPolicyBuilder builder)
        {
            builder.WithOrigins(
                "http://127.0.0.1:7070",
                "http://locahost:7070",
                "http://127.0.0.1:3000",
                "http://localhost:3000",
                "https://lively-pond-069dcf00f.5.azurestaticapps.net"
                ).AllowAnyMethod().AllowAnyHeader()
                .AllowCredentials();
        });
    });

    builder.Services.AddSignalR();
    builder.Services.AddApplicationServices(builder.Configuration);
/*
     builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
   builder.Services.RegisterServices(builder.Configuration);
 *
 */


    var app = builder.Build();
    app.UseRouting();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();



    app.UseAuthorization();

    app.UseCors("CorsPolicy");
    app.UseCors("signalr");


    app.MapControllers();

    var eventBus = app.Services.GetRequiredService<IServiceBusSportAPP>();
    eventBus.ListenerServicesSportApp();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHubSignal>("/chart");
        endpoints.MapControllers();
    });

    app.Run();
