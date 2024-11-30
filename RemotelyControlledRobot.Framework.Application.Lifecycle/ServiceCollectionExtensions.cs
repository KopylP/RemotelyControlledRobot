using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

namespace RemotelyControlledRobot.Framework.Application.Lifecycle;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStartLifecycle<TStartLifecycle>(
        this IServiceCollection services) where TStartLifecycle : class, IStartLifecycle
    {
        services.AddScoped<IStartLifecycle, TStartLifecycle>();
        return services;
    }
    
    public static IServiceCollection AddStopLifecycle<TStopLifecycle>(
        this IServiceCollection services) where TStopLifecycle : class, IStopLifecycle
    {
        services.AddScoped<IStopLifecycle, TStopLifecycle>();
        return services;
    }
}