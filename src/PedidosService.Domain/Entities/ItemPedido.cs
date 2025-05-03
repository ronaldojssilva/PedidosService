using PedidosService.Domain.Exceptions;

namespace PedidosService.Domain.Entities;

public class ItemPedido
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public Pedido Pedido { get; private set; } = null!;
    public int ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    protected ItemPedido() {}

    public ItemPedido(int produtoId, decimal valorUnitario, int quantidade)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;

        if (quantidade <= 0 || valorUnitario <= 0)
            throw new DomainException("Quantidade e valor devem ser positivos.");
    }

    public decimal SubTotal => ValorUnitario * Quantidade;
}
