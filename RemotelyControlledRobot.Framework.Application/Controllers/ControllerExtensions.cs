using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.Extensions;

namespace RemotelyControlledRobot.Framework.Application.Controllers;

internal static class ControllerExtensions
{
    public static IEnumerable<Type> GetControllers(this Type assembly)
        => assembly.Assembly.GetAllImplementingInterface(typeof(IController));

    public static bool ShouldBeRegistered(this Type controller, IConfiguration configuration)
    {
        var attribute = (ControllerEnableAttribute?)Attribute
            .GetCustomAttribute(controller, typeof(ControllerEnableAttribute));
        
        return attribute is null || configuration.GetValue<bool>(attribute.ConfigurationKey);
    }
    
    public static void RegisterController(this IServiceCollection services, Type controller)
        => services.AddSingleton(typeof(IController), controller);
}