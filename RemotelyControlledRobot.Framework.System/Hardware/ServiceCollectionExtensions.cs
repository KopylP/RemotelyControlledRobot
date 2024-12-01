using System.Device.Gpio;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Lifecycle;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

namespace RemotelyControlledRobot.Framework.System.Hardware;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHardware(this IServiceCollection services, Type[] assemblyReferences)
	{
		// TODO: Create an abstraction layer to encapsulate
		// TODO: GpioController functionality, allowing for easier testing and future
		services.AddSingleton(new GpioController(PinNumberingScheme.Logical));

		var hardwareComponents = assemblyReferences
			.SelectMany(p => p.Assembly
				.GetAllImplementingInterface(typeof(IHardware)));

		foreach (var hardwareComponent in hardwareComponents)
		{
			services.AddSingleton(typeof(IHardware), hardwareComponent);
		}

		services.AddScoped<IHardwareProvider, HardwareProvider>();
		services.AddStartLifecycle<HardwareBootstrapper>();
		services.AddStopLifecycle<HardwareBootstrapper>();

		return services;
	}
}