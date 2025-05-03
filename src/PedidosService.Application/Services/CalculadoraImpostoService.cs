using Microsoft.Extensions.Configuration;
using PedidosService.Domain.Services;

namespace PedidosService.Application.Services;

public class CalculadoraImpostoService
{
    private readonly ICalculadoraImposto _calculadora;

    public CalculadoraImpostoService(
        IConfiguration configuration,
        ICalculadoraImposto calculadoraAntiga,
        ICalculadoraImposto calculadoraNova)
    {
        bool usarNovaRegra = bool.TryParse(configuration.GetSection("FeatureFlags:UsarNovaRegraImposto").Value, out var result) && result;
        _calculadora = usarNovaRegra ? calculadoraNova : calculadoraAntiga;
    }

    public decimal Calcular(decimal valorTotal)
        => _calculadora.Calcular(valorTotal);
}
