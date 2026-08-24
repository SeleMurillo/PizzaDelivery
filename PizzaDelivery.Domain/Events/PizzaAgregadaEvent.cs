using PizzaDelivery.Domain.Common;

namespace PizzaDelivery.Domain.Events;

// Evento que se dispara cuando se agrega una pizza al pedido
public class PizzaAgregadaEvent(Guid pedidoId, Guid pizzaId) : DomainEvent
{
    public Guid PedidoId { get; } = pedidoId;
    public Guid PizzaId { get; } = pizzaId;
}
