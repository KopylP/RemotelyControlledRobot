using System;
using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
	internal static class TypeExtentions
	{
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
    }
}

