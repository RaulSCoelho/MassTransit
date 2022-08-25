﻿using Microsoft.Extensions.Hosting;
using MassTransit;
using Components.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // MassTransit
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<SubmitOrderConsumer>();

            cfg.UsingRabbitMq(ConfigureBus);
        });
    })
    .Build();

static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
{
    configurator.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.Consumer<SubmitOrderConsumer>(), e =>
    {
        e.ConfigureConsumer<SubmitOrderConsumer>(context);
    });

    configurator.ConfigureEndpoints(context);
}

await host.RunAsync();