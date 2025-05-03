namespace PedidosService.Application.Dtos;

public record PedidoResponse(int Id, int PedidoId, int ClienteId, decimal Impostos, List<ItemPedidoResponse> Itens, string Status);

public record ItemPedidoResponse(int ProdutoId, int Quantidade, decimal Valor);