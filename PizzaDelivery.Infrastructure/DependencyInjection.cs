using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Infrastructure.Services;
using PizzaDelivery.Infrastructure.Settings;

namespace PizzaDelivery.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Service Bus Client
        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ServiceBusSettings>>().Value;
            return new ServiceBusClient(settings.ConnectionString);
        });

        // Publisher
        services.AddScoped<IEventPublisher, ServiceBusPublisher>();

        // Settings
        services.Configure<ServiceBusSettings>(options =>
            configuration.GetSection(ServiceBusSettings.SectionName).Bind(options));

        return services;
    }
}
