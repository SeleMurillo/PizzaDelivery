using MediatR;
using Microsoft.EntityFrameworkCore;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Domain.Entities;
using PizzaDelivery.Shared.Exceptions;

namespace PizzaDelivery.Application.Pedidos.Commands;

public record ConfirmarPedidoCommand : IRequest<Guid>
{
    public Guid PedidoId { get; init; }
    public List<Guid> PizzaIds { get; init; } = new();
}

public class ConfirmarPedidoHandler : IRequestHandler<ConfirmarPedidoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher _publisher;
    
    public ConfirmarPedidoHandler(IApplicationDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }
    
    public async Task<Guid> Handle(ConfirmarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Pizzas)
            .FirstOrDefaultAsync(p => p.Id == request.PedidoId, cancellationToken);

        if (pedido == null)
            throw new NotFoundException(nameof(Pedido), request.PedidoId);

        // Esto genera PedidoConfirmadoEvent INTERNAMENTE
        pedido.Confirmar();  

        await _context.SaveChangesAsync(cancellationToken);

        // Publicar eventos después de guardar
        foreach (var domainEvent in pedido.DomainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        // Return the Pedido's Id (Guid) instead of MediatR.Unit
        return pedido.Id;
    }
}