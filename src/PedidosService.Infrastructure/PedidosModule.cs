using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PedidosService.Application.Entrega;
using PedidosService.Application.Http;
using PedidosService.Application.Services;
using PedidosService.Domain.Interfaces.Repositories;
using PedidosService.Domain.Services;
using PedidosService.Infrastructure.DbContexts;
using PedidosService.Infrastructure.HostedServices;
using PedidosService.Infrastructure.Repositories;

namespace PedidosService.Infrastructure;

public static class PedidosModule
{
    public static IServiceCollection AddPedidosModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoService, PedidoService>();

        services.AddSingleton<CalculadoraImpostoAntiga>();
        services.AddSingleton<CalculadoraImpostoNova>();
        services.AddScoped<CalculadoraImpostoService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var antiga = sp.GetRequiredService<CalculadoraImpostoAntiga>();
            var nova = sp.GetRequiredService<CalculadoraImpostoNova>();
            return new CalculadoraImpostoService(config, antiga, nova);
        });

        services.AddScoped<IPedidoEntregaService, PedidoEntregaService>();
        services.AddHostedService<PedidoEntregaWorker>();

    }
}
