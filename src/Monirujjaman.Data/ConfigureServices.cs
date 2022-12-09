using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monirujjaman.Data.Contracts;

namespace Monirujjaman.Data;

public static class ConfigureServices
{
    public static void AddRepositoryPattern<TContext>(this IServiceCollection services, Action<ServiceConfiguration>? configuration) where TContext : DbContext
    {
        var serviceConfig = new ServiceConfiguration();
        
        configuration?.Invoke(serviceConfig);
        
        services.TryAdd(new ServiceDescriptor(typeof(IUnitOfWork), typeof(UnitOfWork<TContext>), serviceConfig.Lifetime));
    }
}
