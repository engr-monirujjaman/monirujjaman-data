using Microsoft.Extensions.DependencyInjection;

namespace Monirujjaman.Data;

public class ServiceConfiguration
{
    public ServiceConfiguration()
    {
        Lifetime = ServiceLifetime.Scoped;
    }

    internal ServiceLifetime Lifetime { get; set; }

    public ServiceConfiguration AsSingleton()
    {
        Lifetime = ServiceLifetime.Singleton;
        return this;
    }

    public ServiceConfiguration AsScoped()
    {
        Lifetime = ServiceLifetime.Scoped;
        return this;
    }

    public ServiceConfiguration AsTransient()
    {
        Lifetime = ServiceLifetime.Transient;
        return this;
    }
}