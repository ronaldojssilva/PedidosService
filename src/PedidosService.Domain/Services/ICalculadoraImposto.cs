namespace PedidosService.Domain.Services;

public interface ICalculadoraImposto
{
    decimal Calcular(decimal valorTotal);
}
