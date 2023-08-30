using System.Device.Gpio;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Application.Controllers;
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

        public RobotApplicationBuilder(IServiceCollection services)
        {
            ColoredConsole.WriteLineYellow("Initializing robot...");
            _services = services;
        }

        public RobotApplicationBuilder AddCommandBus()
        {
            ColoredConsole.WriteLineYellow("Registering Command Bus...");
            _services.AddMessageBus();

            return this;
        }

        public RobotApplicationBuilder AddSignalR()
        {
            ColoredConsole.WriteLineYellow("Registering SignalR...");

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.31:5047/robothub") // TODO move to Configuration
                .Build();

            _services.AddSingleton(hubConnection);

            return this;
        }

        public RobotApplicationBuilder RegisterServices()
        {
            ColoredConsole.WriteLineYellow("Registering services...");

            _services.AddSingleton<RobotApplication>();
            _services.AddHardwares(typeof(HardwareBase));
            _services.AddControllers(typeof(ControllerBase));
            _services.AddSingleton(new GpioController(PinNumberingScheme.Logical));
            _services.AddTransient<IServoFactory, ServoFactory>();

            return this;
        }

        public RobotApplication Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<RobotApplication>();
        }
    }
}
