using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaDelivery.Application.Pedidos.Commands;
using PizzaDelivery.Application.Pedidos.DTOs;
using PizzaDelivery.Application.Pedidos.Queries;
using PizzaDelivery.Domain.DTOs;

namespace PizzaDelivery.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PedidosController> _logger;

    public PedidosController(IMediator mediator, ILogger<PedidosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea un nuevo pedido
    /// </summary>
    /// <param name="command">Datos del pedido</param>
    /// <returns>ID del pedido creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> CrearPedido([FromBody] CrearPedidoCommand command)
    {
        try
        {
            var pedidoId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPedido), new { id = pedidoId }, pedidoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando pedido");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un pedido por ID
    /// </summary>
    /// <param name="id">ID del pedido</param>
    /// <returns>Detalles del pedido</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> GetPedido(Guid id)
    {
        var query = new GetPedidoQuery { PedidoId = id };
        var pedido = await _mediator.Send(query);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    /// <summary>
    /// Confirma un pedido (solo cocina)
    /// </summary>
    [HttpPost("{id}/confirmar")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmarPedido(Guid id)
    {
        var command = new ConfirmarPedidoCommand { PedidoId = id };
        await _mediator.Send(command);
        
        return Accepted(new { message = "Pedido confirmado, pasará a preparación" });
    }

    /// <summary>
    /// Lista pedidos con paginación
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PedidoResumenDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<PedidoResumenDto>>> GetPedidos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetPedidosPaginadosQuery
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}