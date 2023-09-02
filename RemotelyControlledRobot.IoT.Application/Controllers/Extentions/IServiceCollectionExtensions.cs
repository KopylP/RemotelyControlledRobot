using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.IoT.Application.Controllers.Extentions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services, Type assembly, IConfiguration configuration)
        {
            foreach (var controller in assembly.GetControllers())
            {
                if (controller.ShouldBeRegistered(configuration))
                    services.RegisterController(controller);
            }

            return services;
        }
    }
}