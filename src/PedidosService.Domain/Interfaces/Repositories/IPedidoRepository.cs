using PedidosService.Domain.Entities;
using PedidosService.Domain.Enums;

namespace PedidosService.Domain.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task AdicionarAsync(Pedido pedido);
    Task<Pedido?> ObterPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ListarPedidosPorStatusAsync(StatusPedido status);
    Task<List<Pedido>> ObterPedidosNaoEnviadosAsync();
    Task AtualizarAsync(Pedido pedido);

}
