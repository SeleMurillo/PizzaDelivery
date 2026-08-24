using PizzaDelivery.Domain.Common;
using PizzaDelivery.Domain.Events;
using PizzaDelivery.Domain.ValueObjects;

namespace PizzaDelivery.Domain.Entities;

public class Pedido : BaseEntity, IAggregateRoot
{
    private readonly List<Pizza> _pizzas = [];
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Direccion DireccionEntrega { get; private set; }
    public IReadOnlyList<Pizza> Pizzas => _pizzas.AsReadOnly();
    public PedidoEstado Estado { get; private set; }
    public decimal Total { get; private set; }

    public Cliente Cliente { get; private set; }

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Pedido() { } // Para EF Core
    
    public Pedido(Guid clienteId, Direccion direccionEntrega)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        DireccionEntrega = direccionEntrega;
        Estado = PedidoEstado.Creado;
        Total = 0;
    }

    protected virtual void AddDomainEvent(DomainEvent domainEvent)
    {
        // Validar que el evento no sea nulo
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

        // Agregar a la lista
        _domainEvents.Add(domainEvent);

        // Opcional: Loggear en desarrollo
#if DEBUG
        Console.WriteLine($"[DOMAIN EVENT] {domainEvent.GetType().Name} agregado a Pedido {Id}");
#endif
    }

    public void AgregarPizza(Pizza pizza)
    {
        if (Estado != PedidoEstado.Creado)
            throw new DomainException("No se puede modificar un pedido en proceso");

        if (_pizzas.Count >= 10)
            throw new DomainException("Máximo 10 pizzas por pedido");

        _pizzas.Add(pizza);
        CalcularTotal();

        var evento = new PizzaAgregadaEvent(Id, pizza.Id);
        AddDomainEvent(evento);  
    }
    
    private void CalcularTotal()
    {
        Total = _pizzas.Sum(p => p.Precio);
    }
    
    public void Confirmar()
    {
        if (!_pizzas.Any())
            throw new DomainException("No se puede confirmar pedido sin pizzas");

        if (Estado != PedidoEstado.Creado)
            throw new DomainException("El pedido ya está en proceso");

        Estado = PedidoEstado.Confirmado;

        var evento = new PedidoConfirmadoEvent(Id, ClienteId, Total);
        AddDomainEvent(evento);
    }

    public void IniciarPreparacion()
    {
        if (Estado != PedidoEstado.Confirmado)
            throw new DomainException("Solo se pueden preparar pedidos confirmados");

        Estado = PedidoEstado.EnPreparacion;                
    }

    public void MarcarEnCamino()
    {
        if (Estado != PedidoEstado.EnPreparacion)
            throw new DomainException("El pedido debe estar preparado");

        Estado = PedidoEstado.EnCamino;
    }

    public void Entregar()
    {
        if (Estado != PedidoEstado.EnCamino)
            throw new DomainException("El pedido debe estar en camino");

        Estado = PedidoEstado.Entregado;

        // 🔥 Evento de pedido entregado
        var evento = new PedidoEntregadoEvent(Id, ClienteId);
        AddDomainEvent(evento);
    }

    public void Cancelar(string motivo)
    {
        if (Estado == PedidoEstado.Entregado)
            throw new DomainException("No se puede cancelar un pedido entregado");

        Estado = PedidoEstado.Cancelado;

        // 🔥 Evento de cancelación
        var evento = new PedidoCanceladoEvent(Id, motivo);
        AddDomainEvent(evento);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public enum PedidoEstado
{
    Creado,
    Confirmado,
    EnPreparacion,
    EnCamino,
    Entregado,
    Cancelado
}