using CrudPedidos.Application.Interfaces;
using CrudPedidos.Application.Services;
using CrudPedidos.Domain.Interfaces;
using CrudPedidos.Infrastructure.Data;
using CrudPedidos.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPedidos.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoService, PedidoService>();

        return services;
    }
}
