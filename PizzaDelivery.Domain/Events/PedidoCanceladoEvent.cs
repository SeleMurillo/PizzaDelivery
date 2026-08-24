using PizzaDelivery.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDelivery.Domain.Events
{
    public class PedidoCanceladoEvent(Guid pedidoId, string motivo) : DomainEvent
    {
        public Guid PedidoId { get; } = pedidoId;
        public string Motivo { get; } = motivo;
    }
}
