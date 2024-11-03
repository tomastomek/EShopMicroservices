using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker
            (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                // sets naming convention for the endpoints
                // this convention is preferred for readibility
                config.SetKebabCaseEndpointNameFormatter();

                // scans assembly and adds consumers if any (no need for publisher)
                if (assembly != null)
                    config.AddConsumers(assembly);

                // use rabbitmq as a transport
                config.UsingRabbitMq((contex, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    // mass transit automatically configures the endpoints for the consumers
                    configurator.ConfigureEndpoints(contex);
                });
            });


            return services;
        }
    }
}