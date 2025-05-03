using PedidosService.Application.Dtos;
using PedidosService.Domain.Enums;

namespace PedidosService.Application.Services;

public interface IPedidoService
{
    Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request);
    Task<PedidoResponse?> ObterPorIdAsync(int id);
    Task<IEnumerable<PedidoResponse>> ObterPorStatusAsync(StatusPedido status);
}