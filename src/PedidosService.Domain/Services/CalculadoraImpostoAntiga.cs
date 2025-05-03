namespace PedidosService.Domain.Services;

public class CalculadoraImpostoAntiga : ICalculadoraImposto
{
    public decimal Calcular(decimal valorTotal)
        => valorTotal * 0.30m;
}
