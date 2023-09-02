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
                    services.AddSingleton(typeof(IController), controller);
            }

            return services;
        }

        private IEnumerable<Type> GetControllersFromAssembly(Assembly assembly)
            => assembly.GetAllImplementingInterface(typeof(IController));
    }
}