using PedidosService.Domain.Enums;
using PedidosService.Domain.Exceptions;

namespace PedidosService.Domain.Entities;

public class Pedido
{
    private Pedido()
    {

    }

    public int Id { get; private set; }
    
    public int PedidoId { get; private set; }

    public int ClienteId { get; private set; }

    public IList<ItemPedido> Itens { get; private set; }

    public StatusPedido Status { get; private set; }

    public DateTime CriadoEm { get; private set; }

    public decimal TotalImpostos { get; private set; }

    public bool EnviadoSistemaB { get; private set; } = false;

    public static Pedido Create(int pedidoId, int clienteId, IEnumerable<ItemPedido> itens)
    {
        if (!itens.Any())
            throw new DomainException("Pedido deve conter pelo menos um item.");
        var pedido = new Pedido
        {
            PedidoId = pedidoId,
            ClienteId = clienteId,
            CriadoEm = DateTime.UtcNow,
            Status = StatusPedido.Criado,
            Itens = itens.ToList(),

        };
        return pedido;
    }

    public void AplicarImpostos(decimal total)
    {
        if (total < 0)
            throw new DomainException("Impostos não podem ser negativos.");
        TotalImpostos = total;
    }

    public void AdicionarItem(ItemPedido item)
    {
        Itens.Add(item);
    }

    public decimal Total()
    {
        return Itens.Sum(i => i.SubTotal);
    }

    public void MarcarComoProcessado()
    {
        Status = StatusPedido.Processado;
    }

    public void MarcarComoEnviado()
    {
        EnviadoSistemaB = true;
    }
}
