using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services, Type assembly)
        {
            var controllers = assembly.Assembly.GetAllImplementingInterface(typeof(IController));

            foreach (var controller in controllers)
            {
                services.AddSingleton(typeof(IController), controller);
            }

            return services;
        }
    }
}

