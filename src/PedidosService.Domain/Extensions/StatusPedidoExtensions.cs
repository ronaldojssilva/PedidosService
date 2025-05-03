using PedidosService.Domain.Enums;

namespace PedidosService.Domain.Extensions;

public static class StatusPedidoExtensions
{
    /// <summary>
    /// Converte uma string para StatusPedido, ignorando maiúsculas/minúsculas.
    /// Retorna null se a conversão falhar.
    /// </summary>
    public static StatusPedido ToStatusPedido(this string status)
    {
        if (Enum.TryParse<StatusPedido>(status, true, out var result))
            return result;

        throw new ArgumentException($"Valor inválido para StatusPedido: '{status}'");
    }

    /// <summary>
    /// Converte um StatusPedido para string (nome do enum).
    /// </summary>
    public static string ToStatusString(this StatusPedido status)
    {
        return status.ToString();
    }
}
