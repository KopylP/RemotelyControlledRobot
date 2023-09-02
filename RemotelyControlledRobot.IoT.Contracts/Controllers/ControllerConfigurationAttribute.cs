using System;
namespace RemotelyControlledRobot.IoT.Contracts.Controllers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]

    public sealed class ControllerConfigurationAttribute : Attribute
	{
		public string ConfigurationKey { get; }

		public ControllerConfigurationAttribute(string configurationKey)
		{
			ConfigurationKey = configurationKey;
		}
	}
}

