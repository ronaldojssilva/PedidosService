namespace ProdutoExternoB.API.Dto;

public record PedidoRequest(int PedidoId, int ClienteId, List<Item> Itens);
public record Item(int ProdutoId, int Quantidade, decimal Valor);
