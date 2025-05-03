using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PedidosService.Application.Entrega;

namespace PedidosService.Infrastructure.HostedServices;

public class PedidoEntregaWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PedidoEntregaWorker> _logger;

    public PedidoEntregaWorker(IServiceProvider serviceProvider, ILogger<PedidoEntregaWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var entregaService = scope.ServiceProvider.GetRequiredService<IPedidoEntregaService>();

            await entregaService.EntregarPedidosNaoEnviadosAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); 
        }
    }
}
