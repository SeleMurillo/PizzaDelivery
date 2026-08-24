using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaDelivery.Domain.Entities
{
    [Table("Pizza")]
    public class Pizza
    {
        [Key]
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
