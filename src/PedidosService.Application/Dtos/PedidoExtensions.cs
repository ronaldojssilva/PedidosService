using PedidosService.Domain.Entities;
using PedidosService.Domain.Extensions;

namespace PedidosService.Application.Dtos;

public static class PedidoExtensions
{
    public static PedidoResponse ToResponse(this Pedido pedido)
        => new(
            pedido.Id,
            pedido.PedidoId,    
            pedido.ClienteId,
            pedido.TotalImpostos,
            pedido.Itens.Select(i => 
                new ItemPedidoResponse(
                    i.ProdutoId,
                    i.Quantidade,
                    i.ValorUnitario)).ToList(),
            pedido.Status.ToStatusString());
}