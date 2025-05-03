using FluentAssertions;
using Microsoft.Extensions.Configuration;
using PedidosService.Application.Services;
using PedidosService.Domain.Services;

namespace PedidosService.UnitTests.Application.Services;

public class CalculadoraImpostoServiceTests
{
    [Theory]
    [InlineData(100, false, 30)]
    [InlineData(100, true, 20)]
    public void Deve_calcular_imposto_corretamente(decimal valor, bool usarNova, decimal esperado)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
            { "FeatureFlags:UsarNovaRegraImposto", usarNova.ToString() }
            })
            .Build();

        var antiga = new CalculadoraImpostoAntiga();
        var nova = new CalculadoraImpostoNova();

        var service = new CalculadoraImpostoService(config, antiga, nova);

        var resultado = service.Calcular(valor);

        resultado.Should().Be(esperado);
    }
}
