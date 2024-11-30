using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RemotelyControlledRobot.Framework.Application.Controllers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllers(this IServiceCollection services, IConfiguration configuration, params Type[] assemblyMarkers)
    {
        var controllers = assemblyMarkers.SelectMany(assembly => assembly.GetControllers());
        foreach (var controller in controllers)
        {
            if (controller.ShouldBeRegistered(configuration)) 
                services.RegisterController(controller);
        }

        return services;
    }
}