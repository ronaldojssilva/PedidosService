using Microsoft.Extensions.Logging;
using PedidosService.Application.Dtos;
using PedidosService.Application.Http;
using PedidosService.Domain.Entities;
using PedidosService.Domain.Interfaces.Repositories;
using System.Net.Http.Json;

namespace PedidosService.Application.Entrega;

public interface IPedidoEntregaService
{
    Task EntregarPedidosNaoEnviadosAsync(CancellationToken cancellationToken);
}

public class PedidoEntregaService : IPedidoEntregaService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<PedidoEntregaService> _logger;

    public PedidoEntregaService(IPedidoRepository pedidoRepository, IHttpClientService httpClient, ILogger<PedidoEntregaService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task EntregarPedidosNaoEnviadosAsync(CancellationToken cancellationToken)
    {
        var pedidos = await _pedidoRepository.ObterPedidosNaoEnviadosAsync();

        foreach (var pedido in pedidos)
        {
            try
            {
                var pedidoDto = pedido.ToResponse(); 
                var response = await _httpClient.PostAsJsonAsync("/api/receber", pedidoDto, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    pedido.MarcarComoEnviado();
                    await _pedidoRepository.AtualizarAsync(pedido);
                }
                else
                {
                    _logger.LogWarning("Falha ao enviar pedido {Id}: {StatusCode}", pedido.Id, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar enviar pedido {Id} para o Sistema B", pedido.Id);
            }
        }
    }
}

public record PedidoParaEnvioDto(int PedidoId, int ClienteId, List<ItemDto> Itens);
public record ItemDto(int ProdutoId, int Quantidade, decimal Valor);

static class ClassPedidoParaEnvioDto
{
    public static PedidoParaEnvioDto ToEnvioDto(this Pedido pedido) =>
        new(pedido.Id, pedido.ClienteId, pedido.Itens.Select(i => new ItemDto(i.ProdutoId, i.Quantidade, i.ValorUnitario)).ToList());
}