using PizzaDelivery.Domain.Common;

namespace PizzaDelivery.Domain.Events;

public class PedidoConfirmadoEvent(Guid pedidoId, Guid clienteId, decimal total) : DomainEvent, IEvent
{
    public Guid PedidoId { get; } = pedidoId;
    public Guid ClienteId { get; } = clienteId;
    public decimal Total { get; } = total;

    public Guid Id => EventId; 
    public string EventType => nameof(PedidoConfirmadoEvent);
}
