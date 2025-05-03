namespace PedidosService.Domain.Services;

public class CalculadoraImpostoNova : ICalculadoraImposto
{
    public decimal Calcular(decimal valorTotal)
        => valorTotal * 0.20m;
}