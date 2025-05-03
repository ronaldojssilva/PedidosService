using Microsoft.Extensions.Logging;
using PedidosService.Application.Dtos;
using PedidosService.Domain.Entities;
using PedidosService.Domain.Interfaces.Repositories;
using PedidosService.Domain.Enums;

namespace PedidosService.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repo;
    private readonly CalculadoraImpostoService _CalculadoraImpostoService;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(IPedidoRepository repo, CalculadoraImpostoService calculadoraImpostoService, ILogger<PedidoService> logger)
    {
        _repo = repo;
        _CalculadoraImpostoService = calculadoraImpostoService;
        _logger = logger;
    }

    public async Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request)
    {
        List<ItemPedido> itens = request.Itens.Select(i => new ItemPedido(i.ProdutoId, i.Valor, i.Quantidade)).ToList();
        Pedido pedido = Pedido.Create(request.PedidoId, request.ClienteId, itens);

        _logger.LogInformation("Calculando saldo do pedido {Sequencial}", pedido.Id);
        pedido.AplicarImpostos(_CalculadoraImpostoService.Calcular(pedido.Total()));

        await _repo.AdicionarAsync(pedido);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse?> ObterPorIdAsync(int id)
    {
        Pedido? pedido = await _repo.ObterPorIdAsync(id);
        return pedido?.ToResponse();
    }

    public async Task<IEnumerable<PedidoResponse>> ObterPorStatusAsync(StatusPedido status)
    {
        var pedidos = await _repo.ListarPedidosPorStatusAsync(status);
        return pedidos.Select(p => p.ToResponse());
    }
}