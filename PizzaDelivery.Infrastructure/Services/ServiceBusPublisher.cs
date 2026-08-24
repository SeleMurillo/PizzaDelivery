using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Domain.Common;
using PizzaDelivery.Infrastructure.Settings;
using System.Text.Json;

namespace PizzaDelivery.Infrastructure.Services;

public class ServiceBusPublisher : IEventPublisher
{
    private readonly ServiceBusSender _sender;
    private readonly ILogger<ServiceBusPublisher> _logger;
    
    public ServiceBusPublisher(
        ServiceBusClient client,
        IOptions<ServiceBusSettings> settings,
        ILogger<ServiceBusPublisher> logger)
    {
        _sender = client.CreateSender(settings.Value.TopicName);
        _logger = logger;
    }
    
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) 
        where T : DomainEvent
    {
        try
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(@event))
            {
                MessageId = @event.Id.ToString(),
                Subject = @event.GetType().Name,
                ContentType = "application/json",
                ApplicationProperties =
                {
                    ["EventType"] = @event.GetType().Name,
                    ["OccurredOn"] = @event.OccurredOn
                }
            };
            
            await _sender.SendMessageAsync(message, cancellationToken);
            
            _logger.LogInformation("Evento publicado: {EventType} - {EventId}", 
                @event.GetType().Name, @event.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publicando evento {EventType}", @event.GetType().Name);
            throw;
        }
    }

    public Task PublishAsync<T>(T eventData, string subject, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        throw new NotImplementedException();
    }

    public Task PublishToQueueAsync<T>(T eventData, string queueName, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        throw new NotImplementedException();
    }
}