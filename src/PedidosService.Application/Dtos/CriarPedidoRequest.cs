namespace PedidosService.Application.Dtos;

public record CriarPedidoRequest(int PedidoId, int ClienteId, List<ItemPedidoRequest> Itens);

public record ItemPedidoRequest(int ProdutoId, int Quantidade, decimal Valor);
