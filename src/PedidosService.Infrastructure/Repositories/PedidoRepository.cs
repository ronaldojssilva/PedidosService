using Microsoft.EntityFrameworkCore;
using PedidosService.Domain.Entities;
using PedidosService.Domain.Interfaces.Repositories;
using PedidosService.Domain.Enums;
using PedidosService.Infrastructure.DbContexts;

namespace PedidosService.Infrastructure.Repositories;

public class PedidoRepository(ApplicationDbContext context) : IPedidoRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AdicionarAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Pedido>> ListarPedidosPorStatusAsync(StatusPedido status)
    {
        return await _context.Pedidos
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task<Pedido?> ObterPorIdAsync(int id) 
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Pedido>> ObterPedidosNaoEnviadosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .Where(p => !p.EnviadoSistemaB)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
    }
}
