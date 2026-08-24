namespace PizzaDelivery.Domain.DTOs
{
    public class PedidoResumenDto
    {
        public Guid Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public int CantidadPizzas { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Hora => Fecha.ToString("HH:mm");
    }
}
