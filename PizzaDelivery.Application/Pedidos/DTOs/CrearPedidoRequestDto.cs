namespace PizzaDelivery.Application.Pedidos.DTOs
{
    public class CrearPedidoRequestDto
    {
        public Guid ClienteId { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Colonia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public List<Guid> PizzaIds { get; set; } = new();
    }
}
