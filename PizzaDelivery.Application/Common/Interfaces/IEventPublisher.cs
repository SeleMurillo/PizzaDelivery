using PizzaDelivery.Domain.Common;

namespace PizzaDelivery.Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T eventData, string subject, CancellationToken cancellationToken = default)
        where T : DomainEvent;

        // Opcional: publicar directamente a una cola
        Task PublishToQueueAsync<T>(T eventData, string queueName, CancellationToken cancellationToken = default)
            where T : DomainEvent;
    }
}
