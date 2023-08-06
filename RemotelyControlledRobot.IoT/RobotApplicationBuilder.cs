using System;
using System.Device.Gpio;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;
using RemotelyControlledRobot.IoT.Infrastructure.Commands;
using RemotelyControlledRobot.IoT.Infrastructure.Controllers;

namespace RemotelyControlledRobot.IoT.Core
{
	internal class RobotApplicationBuilder
	{
		private readonly IServiceCollection _services;
		public RobotApplicationBuilder(IServiceCollection services) => _services = services;

		public RobotApplicationBuilder RegisterGpioController()
		{
			var gpioController = new GpioController();
			_services.AddSingleton(gpioController);

			return this;
		}

        public RobotApplicationBuilder RegisterServices()
		{
            ColoredConsole.WriteLineYellow("Registering services...");

            _services.AddSingleton<IInternalCommandPublisher, InternalCommandPublisher>();
            _services.AddSingleton<ICommandQueue, MemoryCommandQueue>();
            _services.AddSingleton<ICommandBus, CommandBus>();
            _services.AddSingleton<RobotApplication>();
			_services.AddSingleton<IDriver, Driver>();
			_services.AddSingleton<IMoveController, MoveController>();

            ColoredConsole.WriteLineGreen("Services has been registered successfully.");

			return this;
        }

		public RobotApplication Build()
		{
			var serviceProvider = _services.BuildServiceProvider();
			return serviceProvider.GetRequiredService<RobotApplication>();
		}
	}
}

