using PedidosService.Application.Dtos;
using System.Threading.Channels;

namespace PedidosService.Application.Services;

public class PedidoProcessor
{
    private readonly Channel<CriarPedidoRequest> _channel;
    public ChannelReader<CriarPedidoRequest> Reader => _channel.Reader;
    public ChannelWriter<CriarPedidoRequest> Writer => _channel.Writer;

    public PedidoProcessor()
    {
        _channel = Channel.CreateUnbounded<CriarPedidoRequest>();
    }
}
