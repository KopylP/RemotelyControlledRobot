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
            var controllers = assembly.Assembly.GetAllImplementingInterface(typeof(IController));

            foreach (var controller in controllers)
            {
                if (ShouldBeRegistered(controller, configuration))
                    services.AddSingleton(typeof(IController), controller);
            }

            return services;
        }

        public static bool ShouldBeRegistered(Type controller, IConfiguration configuration)
        {
            var settings = GetControllerSettings(controller, configuration);
            return !settings.IsDisabled;
        }

        public static ControllerSettings GetControllerSettings(Type controller, IConfiguration configuration)
        {
            var attribute = (ControllerConfigurationAttribute?)Attribute
                .GetCustomAttribute(controller, typeof(ControllerConfigurationAttribute));

            var controllerSettings = new ControllerSettings();

            if (attribute is not null)
                configuration.GetSection(attribute.ConfigurationKey).Bind(controllerSettings);

            return controllerSettings;
        }
    }
}

