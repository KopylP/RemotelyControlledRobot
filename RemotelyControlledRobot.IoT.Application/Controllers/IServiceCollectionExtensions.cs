using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services, Type assembly, IConfiguration configuration)
        {
            foreach (var controller in GetControllersFromAssembly(assembly.Assembly))
            {
                if (controller.ShouldBeRegistered(configuration))
                    services.RegisterController(controller);
            }

            return services;
        }

        private static IEnumerable<Type> GetControllersFromAssembly(Assembly assembly)
            => assembly.GetAllImplementingInterface(typeof(IController));

        private static void RegisterController(this IServiceCollection services, Type controller)
            => services.AddSingleton(typeof(IController), controller);
    }
}