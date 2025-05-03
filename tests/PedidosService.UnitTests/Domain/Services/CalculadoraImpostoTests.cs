using FluentAssertions;
using PedidosService.Domain.Services;

namespace PedidosService.UnitTests.Domain.Services;

public class CalculadoraImpostoTests
{
    [Theory]
    [InlineData(100, 30)]
    [InlineData(200, 60)]
    public void CalculadoraImpostoAntiga_Deve_Calcular_Imposto_Antigo_Corretamente(decimal valor, decimal esperado)
    {
        var strategy = new CalculadoraImpostoAntiga();
        var resultado = strategy.Calcular(valor);
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData(100, 20)]
    [InlineData(200, 40)]
    public void CalculadoraImpostoNova_Deve_Calcular_Imposto_Antigo_Corretamente(decimal valor, decimal esperado)
    {
        var strategy = new CalculadoraImpostoNova();
        var resultado = strategy.Calcular(valor);
        resultado.Should().Be(esperado);
    }
}
