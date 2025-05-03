using BenchmarkDotNet.Attributes;
using System.Text.Json;
using System.Text;
using PedidosService.Application.Dtos;

[MemoryDiagnoser]
public class PedidoBenchmark
{
    private HttpClient _client;

    [GlobalSetup]
    public void Setup()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    [Benchmark]
    public async Task EnviarPedidos()
    {
        for (int i = 0; i < 1; i++)
        {
            var itens = new List<ItemPedidoRequest>
        {
            new ItemPedidoRequest(1, 2, 50.0m),
            new ItemPedidoRequest(3, 1, 30.0m),
            new ItemPedidoRequest(9, 3, 20.0m)
        };

            var criarRequest = new CriarPedidoRequest(
                i,  
                i,  
                itens
            );

            var content = new StringContent(
                JsonSerializer.Serialize(criarRequest),
                Encoding.UTF8,
                "application/json"
            );

            await _client.PostAsync("/api/pedido", content);
        }
    }
}
