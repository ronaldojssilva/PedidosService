using Microsoft.AspNetCore.Mvc;
using PedidosService.Application.Dtos;
using PedidosService.Application.Services;
using PedidosService.Domain.Extensions;

namespace PedidosService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoRequest request)
    {
        if (request == null)
        {
            return BadRequest("Pedido inválido.");
        }

        PedidoResponse pedidoResponse = await _pedidoService.CriarPedidoAsync(request);
        return StatusCode(StatusCodes.Status201Created, new { pedidoResponse.Id, pedidoResponse.Status});
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoResponse>> ObterPorId(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);
        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResponse>>> ObterPorStatus([FromQuery] string status)
    {
        var pedidos = await _pedidoService.ObterPorStatusAsync(status.ToStatusPedido());
        return Ok(pedidos);
    }
}
