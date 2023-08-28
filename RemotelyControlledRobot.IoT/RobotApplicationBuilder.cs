using System;
using System.Device.Gpio;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Application.Controllers;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Hardware;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Engines;
using RemotelyControlledRobot.IoT.Infrastructure.Commands;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Engines;

namespace RemotelyControlledRobot.IoT.Core
{
    internal class RobotApplicationBuilder
    {
        private readonly IServiceCollection _services;
        public RobotApplicationBuilder(IServiceCollection services) => _services = services;

        public RobotApplicationBuilder AddSignalR()
        {
            ColoredConsole.WriteLineYellow("Adding SignalR...");

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.31:5047/robothub") // TODO move to Configuration
                .Build();

            _services.AddSingleton(hubConnection);

            ColoredConsole.WriteLineGreen("SignalR added successfully.");

            return this;
        }

        public RobotApplicationBuilder RegisterServices()
        {
            ColoredConsole.WriteLineYellow("Registering services...");

            _services.AddSingleton<ICommandPublisher, InternalCommandPublisher>();
            _services.AddSingleton<ICommandQueue, MemoryCommandQueue>();
            _services.AddSingleton<ICommandBus, CommandBus>();
            _services.AddSingleton<RobotApplication>();
            _services.AddHardwares(typeof(HardwareBase));
            _services.AddControllers(typeof(ControllerBase));
            _services.AddSingleton(new GpioController(PinNumberingScheme.Logical));
            _services.AddTransient<IServoFactory, ServoFactory>();

            ColoredConsole.WriteLineGreen("Services registered successfully.");

            return this;
        }

        public RobotApplication Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<RobotApplication>();
        }
    }
}
