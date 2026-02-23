using CrudOrders.Application.Interfaces;
using CrudOrders.Application.Services;
using CrudOrders.Domain.Interfaces;
using CrudOrders.Infrastructure.Data;
using CrudOrders.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CrudOrders.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
