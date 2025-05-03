using Microsoft.Extensions.DependencyInjection;
using PedidosService.Application.Http;
using Testcontainers.PostgreSql;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.RequestBuilders;
using FluentAssertions;
using Bogus;
using PedidosService.Application.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using PedidosService.Infrastructure.HostedServices;
using PedidosService.Application.Entrega;

namespace PedidosService.IntegrationTests;

public class EntregaPedidoTests : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer;
    private readonly WireMockServer _mockSistemaB;

    public EntregaPedidoTests()
    {
        _pgContainer = new PostgreSqlBuilder()
       .WithImage("postgres:latest")
       .WithDatabase("pedidosservice")
       .WithUsername("postgres")
       .WithPassword("postgres")
       .Build();

        _mockSistemaB = WireMockServer.Start();
    }

    public async Task InitializeAsync()
    {
        await _pgContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _pgContainer.StopAsync();
        _mockSistemaB.Stop();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var overrideSettings = new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = _pgContainer.GetConnectionString(),
                ["ExternalServices:SistemaB"] = _mockSistemaB!.Url
            };

            config.AddInMemoryCollection(overrideSettings);
        });


        // Remover o PedidoEntregaWorker
        builder.ConfigureServices(services =>
        {
            var workerDescriptor = services.SingleOrDefault(d => d.ImplementationType == typeof(PedidoEntregaWorker));
            if (workerDescriptor is not null)
            {
                services.Remove(workerDescriptor);
            }
        });
    }

    [Fact]
    public async Task Deve_Enviar_Pedidos_Calculados_Para_Sistema_B()
    {
        // Arrange: mock do sistema B
        _mockSistemaB
            .Given(Request.Create().WithPath("/api/receber").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(200));

        var client = CreateClient();

        // Usa o serviço real para criar pedido
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

        using var scope = Services.CreateScope();
        var entregaService = scope.ServiceProvider.GetRequiredService<IPedidoEntregaService>();

        // Act
        await entregaService.EntregarPedidosNaoEnviadosAsync(CancellationToken.None);

        // Assert
        var pedidosEnviados = _mockSistemaB.LogEntries;
        pedidosEnviados.Should().ContainSingle();
    }
}

