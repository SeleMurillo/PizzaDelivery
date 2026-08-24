using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaDelivery.Application.Common.Interfaces;
using PizzaDelivery.Application.Pedidos.DTOs;
using PizzaDelivery.Domain.DTOs;
using PizzaDelivery.Domain.Entities;

namespace PizzaDelivery.Application.Pedidos.Queries
{
    public record GetPedidosPaginadosQuery : IRequest<PagedResult<PedidoResumenDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? Estado { get; init; }  // Filtro opcional por estado
        public DateTime? Desde { get; init; }  // Filtro opcional por fecha
        public DateTime? Hasta { get; init; }
        public string? Buscar { get; init; }  // Búsqueda por cliente
    }

    public class GetPedidosPaginadosHandler : IRequestHandler<GetPedidosPaginadosQuery, PagedResult<PedidoResumenDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPedidosPaginadosHandler> _logger;

        public GetPedidosPaginadosHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ILogger<GetPedidosPaginadosHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<PedidoResumenDto>> Handle(
            GetPedidosPaginadosQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obteniendo pedidos paginados: Página {Page}, Tamaño {PageSize}",
                request.PageNumber, request.PageSize);

            // Validar parámetros de paginación
            var pageNumber = Math.Max(1, request.PageNumber);
            var pageSize = Math.Clamp(request.PageSize, 1, 100);  // Máximo 100 items por página

            // 1. CONSTRUIR LA CONSULTA BASE (con AsNoTracking para performance)
            var query = _context.Pedidos
                .AsNoTracking()  // 🚀 Solo lectura
                .Include(p => p.Cliente)
                .Include(p => p.Pizzas)
                .AsQueryable();

            // 2. APLICAR FILTROS (si vienen en el request)
            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                if (Enum.TryParse<PedidoEstado>(request.Estado, true, out var estado))
                {
                    query = query.Where(p => p.Estado == estado);
                }
            }

            if (request.Desde.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= request.Desde.Value);
            }

            if (request.Hasta.HasValue)
            {
                var hasta = request.Hasta.Value.AddDays(1);  // Incluir todo el día
                query = query.Where(p => p.CreatedAt <= hasta);
            }

            if (!string.IsNullOrWhiteSpace(request.Buscar))
            {
                var busqueda = request.Buscar.ToLower();
                query = query.Where(p =>
                    p.Cliente != null &&
                    p.Cliente.Nombre.ToLower().Contains(busqueda));
            }

            // 3. CONTAR TOTAL DE REGISTROS (para la paginación)
            var totalCount = await query.CountAsync(cancellationToken);

            // 4. APLICAR ORDENAMIENTO Y PAGINACIÓN
            var items = await query
                .OrderByDescending(p => p.CreatedAt)  // Más recientes primero
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // 5. MAPEAR A DTO
            var dtos = _mapper.Map<List<PedidoResumenDto>>(items);

            // 6. CREAR RESULTADO PAGINADO
            var result = new PagedResult<PedidoResumenDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            _logger.LogInformation("Se obtuvieron {Count} pedidos de {TotalCount} totales",
                dtos.Count, totalCount);

            return result;
        }
    }
}
