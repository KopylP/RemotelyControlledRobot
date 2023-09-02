using System.Device.Gpio;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Application.Controllers;
using RemotelyControlledRobot.IoT.Contracts.Hardware;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Engines;
using RemotelyControlledRobot.IoT.Infrastructure.Commands;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Engines;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Core
{
    internal class RobotApplicationBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public RobotApplicationBuilder(IServiceCollection services, IConfiguration configuration)
        {
            ColoredConsole.WriteLineYellow("Initializing robot...");
            _services = services;
            _configuration = configuration;
        }

        public RobotApplicationBuilder RegisterConfiguration()
        {
            var cameraNeckSettings = new CameraNeckSettings();
            var driverSettings = new DriverSettings();

            _configuration.GetSection(CameraNeckSettings.Section).Bind(cameraNeckSettings);
            _configuration.GetSection(DriverSettings.Section).Bind(driverSettings);

            _services.AddSingleton(cameraNeckSettings);
            _services.AddSingleton(driverSettings);

            return this;
        }

        public RobotApplicationBuilder AddCommandBus()
        {
            ColoredConsole.WriteLineYellow("Registering Command Bus...");
            _services.AddCommandBus();

            return this;
        }

        public RobotApplicationBuilder AddSignalR()
        {
            ColoredConsole.WriteLineYellow("Registering SignalR...");

            var hubConnection = new HubConnectionBuilder()
                .WithUrl(_configuration["SignalRHost"]!)
                .WithAutomaticReconnect(retryPolicy: )
                .Build();

            _services.AddSingleton(hubConnection);

            return this;
        }

        public RobotApplicationBuilder RegisterServices()
        {
            ColoredConsole.WriteLineYellow("Registering services...");

            _services.AddSingleton<RobotApplication>();
            _services.AddHardwares(typeof(HardwareBase));
            _services.AddControllers(typeof(ControllerBase), _configuration);
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
