using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers.Extentions
{
    internal static class ControllerExtensions
    {
        public static IEnumerable<Type> GetControllers(this Type assembly)
            => assembly.Assembly.GetAllImplementingInterface(typeof(IController));

        public static bool ShouldBeRegistered(this Type controller, IConfiguration configuration)
        {
            var settings = controller.GetControllerSettings(configuration);
            return !settings.IsDisabled;
        }

        private static ControllerSettings GetControllerSettings(this Type controller, IConfiguration configuration)
        {
            var attribute = (ControllerConfigurationAttribute?)Attribute
                .GetCustomAttribute(controller, typeof(ControllerConfigurationAttribute));

            var controllerSettings = new ControllerSettings();

            if (attribute is not null)
                configuration.GetSection(attribute.ConfigurationKey).Bind(controllerSettings);

            return controllerSettings;
        }

        public static void RegisterController(this IServiceCollection services, Type controller)
            => services.AddSingleton(typeof(IController), controller);
    }
}

