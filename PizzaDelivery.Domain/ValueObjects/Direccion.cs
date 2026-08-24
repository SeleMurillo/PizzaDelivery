namespace PizzaDelivery.Domain.ValueObjects;

public record Direccion
{
    public string Calle { get; }
    public string Numero { get; }
    public string Colonia { get; }
    public string Ciudad { get; }
    
    public Direccion(string calle, string numero, string colonia, string ciudad)
    {
        if (string.IsNullOrWhiteSpace(calle))
            throw new ArgumentException("La calle es requerida", nameof(calle));
            
        Calle = calle;
        Numero = numero;
        Colonia = colonia;
        Ciudad = ciudad;
    }
}