using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Lifecycle;
using RemotelyControlledRobot.Framework.System.Bus;
using RemotelyControlledRobot.Framework.System.Hardware;

namespace RemotelyControlledRobot.Framework.System;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSystem(this IServiceCollection services, params Type[] assemblyReferences)
    {
        services.AddHardware(assemblyReferences);
        services.AddCommandBus();

        return services;
    }
}