using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware
{
	public static class IServiceCollectionExtensions
    {
		public static IServiceCollection AddHardwares(this IServiceCollection services, Type assembly)
		{
			var hardwareComponents = assembly.Assembly.GetAllImplementingInterface(typeof(IHardware));

			foreach (var hardwareComponent in hardwareComponents)
			{
				services.AddSingleton(typeof(IHardware), hardwareComponent);
			}

			services.AddScoped<IHardwareProvider, HardwareProvider>();
			services.AddScoped<IHardwareBootstrap, HardwareBootstrap>();

            return services;
		}
	}
}

