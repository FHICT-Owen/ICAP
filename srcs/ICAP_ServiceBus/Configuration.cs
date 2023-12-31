﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ICAP_ServiceBus
{
    public static class Configuration
    {
        public static void UseAzureServiceBusHandler(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IBusHandler>(_ =>
                new BusHandler(config["AzureServiceBus"] ?? throw new ArgumentNullException()));
        }

        public static void UseAzureServiceBusPublisher(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IBusPublisher>(_ =>
                new BusPublisher(config["AzureServiceBus"] ?? throw new ArgumentNullException()));
        }
    }
}