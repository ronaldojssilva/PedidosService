using Microsoft.AspNetCore.Mvc;
using ProdutoExternoB.API.Dto;

namespace ProdutoExternoB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceberController : ControllerBase
    {
        private readonly ILogger<ReceberController> _logger;

        public ReceberController(ILogger<ReceberController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReceberPedido([FromBody] PedidoRequest request)
        {
            if (request == null)
            {
                return BadRequest("Pedido inválido.");
            }

            return Ok();
        }

    }
}
