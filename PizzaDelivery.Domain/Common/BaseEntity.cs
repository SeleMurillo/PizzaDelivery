using System.Collections.Generic;
using MediatR;

namespace PizzaDelivery.Domain.Common
{
    public abstract class BaseEntity
    {
        private readonly List<INotification> _domainEvents = new();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(INotification eventItem)
        {
            if (eventItem is null) return;
            _domainEvents.Add(eventItem);
        }

        protected void RemoveDomainEvent(INotification eventItem)
        {
            if (eventItem is null) return;
            _domainEvents.Remove(eventItem);
        }

        protected void ClearDomainEvents() => _domainEvents.Clear();
    }
}