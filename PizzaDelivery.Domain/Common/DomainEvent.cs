namespace PizzaDelivery.Domain.Common
{
    // Evento de dominio base
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
        Guid EventId { get; }
    }

    // Implementación base de evento
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid EventId { get; } = Guid.NewGuid();
        public Guid Id { get; set; }
    }
}
