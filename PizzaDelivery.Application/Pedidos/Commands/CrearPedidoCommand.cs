using MediatR;
using Microsoft.EntityFrameworkCore;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Domain.Entities;
using PizzaDelivery.Domain.ValueObjects;

namespace PizzaDelivery.Application.Pedidos.Commands;

public record CrearPedidoCommand : IRequest<Guid>
{
    public Guid ClienteId { get; init; }
    public string Calle { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string Colonia { get; init; } = string.Empty;
    public string Ciudad { get; init; } = string.Empty;
    public List<Guid> PizzaIds { get; init; } = new();
}

public class CrearPedidoHandler(IApplicationDbContext context, IPublisher publisher) : IRequestHandler<CrearPedidoCommand, Guid>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IPublisher _publisher = publisher;

    public async Task<Guid> Handle(CrearPedidoCommand request, CancellationToken cancellationToken)
    {
        // Crear Value Object
        var direccion = new Direccion(
            request.Calle, 
            request.Numero, 
            request.Colonia, 
            request.Ciudad);
        
        // Crear entidad
        var pedido = new Pedido(request.ClienteId, direccion);
        
        // Agregar pizzas
        var pizzas = await _context.Pizzas
            .Where(p => request.PizzaIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
            
        foreach (var pizza in pizzas)
        {
            pedido.AgregarPizza(pizza);
        }
        
        // Guardar
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Publicar eventos
        foreach (var evento in pedido.DomainEvents)
        {
            await _publisher.Publish(evento, cancellationToken);
        }
        
        return pedido.Id;
    }
}