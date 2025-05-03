using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using PedidosService.Application.Dtos;
using PedidosService.Domain.Extensions;
using PedidosService.Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json;
using Testcontainers.PostgreSql;

namespace PedidosService.IntegrationTests;
public class PedidoEndpointTests : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres;

    public PedidoEndpointTests()
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("pedidosservice")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnection", _postgres.GetConnectionString());
    }

    public async Task InitializeAsync() => await _postgres.StartAsync();
    public new async Task DisposeAsync() => await _postgres.DisposeAsync();

    [Fact]
    public async Task Deve_Criar_E_Consultar_Pedido_Com_Sucesso()
    {
        var client = CreateClient();

        var faker = new Faker<CriarPedidoRequest>()
            .CustomInstantiator(f => new CriarPedidoRequest(
                f.Random.Int(1, 99999),
                f.Random.Int(1, 999),
                new List<ItemPedidoRequest>
                {
                    new ItemPedidoRequest(
                        f.Random.Int(1000, 2000),
                        f.Random.Int(1, 5),
                        f.Random.Decimal(10, 100)
                    )
                }
            ));
        CriarPedidoRequest pedido = faker.Generate(1).First();

        var response = await client.PostAsJsonAsync("/api/pedido", pedido);

        var contentStream = await response.Content.ReadAsStreamAsync();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content); // ou use um breakpoint/log

        response.EnsureSuccessStatusCode();

        var postResult = await JsonSerializer.DeserializeAsync<PedidoResponse>(contentStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var location = $"/api/pedido/{postResult!.Id}";
        PedidoResponse? consulta = await client.GetFromJsonAsync<PedidoResponse>(location);

        consulta.Should().NotBeNull();
        consulta!.Id.Should().BeGreaterThan(0);
        consulta.Status.Should().Be(StatusPedido.Criado.ToStatusString());
    }
}