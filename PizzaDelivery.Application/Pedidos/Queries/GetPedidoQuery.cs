using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Application.Pedidos.DTOs;
using PizzaDelivery.Domain.Entities;
using PizzaDelivery.Shared.Exceptions;

namespace PizzaDelivery.Application.Pedidos.Queries
{
    public class GetPedidoQuery : IRequest<PedidoDto>
    {
        public Guid PedidoId { get; init; }

    }

    public class GetPedidoHandler(IApplicationDbContext context,
    IMapper mapper,
    ILogger<GetPedidoHandler> logger) : IRequestHandler<GetPedidoQuery, PedidoDto>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPedidoHandler> _logger = logger;

        public async Task<PedidoDto> Handle(GetPedidoQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obteniendo pedido {PedidoId}", request.PedidoId);

            var pedido = await _context.Pedidos
            .AsNoTracking()  
            .Include(p => p.Pizzas)  
            .Include(p => p.Cliente)  
            .FirstOrDefaultAsync(p => p.Id == request.PedidoId, cancellationToken);

            if (pedido == null)
            {
                _logger.LogWarning("Pedido {PedidoId} no encontrado", request.PedidoId);
                throw new NotFoundException(nameof(Pedido), request.PedidoId);
            }
                        
            var dto = _mapper.Map<PedidoDto>(pedido);

            _logger.LogInformation("Pedido {PedidoId} obtenido exitosamente", request.PedidoId);

            return dto;
        }
    }
}
