namespace PizzaDelivery.Application.Pedidos.DTOs
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public List<string> Pizzas { get; set; } = new();
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}
