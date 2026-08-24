using PizzaDelivery.Domain.ValueObjects;

namespace PizzaDelivery.Domain.Entities
{
    public class Cliente
    {
        public Cliente()
        {
            Nombre = string.Empty;
        }
        public Guid ClienteId { get; set; }
        public string Nombre { get; set; }
        public bool IS_ACTIVE { get; set; }
        public Direccion Direccion { get; set; } = null!;
        public Guid CREATED_BY_OPERATOR_ID { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public Guid? MODIFIED_BY_OPERATOR_ID { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
    }
}
