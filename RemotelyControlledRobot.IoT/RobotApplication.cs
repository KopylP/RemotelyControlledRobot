using System.Device.Gpio;
using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Core
{
    public class RobotApplication
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly IEnumerable<IController> _controllers;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IHardwareBootstrap _hardwareBootstrap;
        private readonly HubConnection _hubConnection;
        private readonly ICommandBus _commandBus;

        public RobotApplication(
            IEnumerable<IController> controllers,
            ICommandPublisher commandPublisher,
            IHardwareBootstrap hardwareBootstrap,
            HubConnection hubConnection,
            ICommandSubscriber commandSubscriber,
            ICommandBus commandBus)
        {
            _controllers = controllers;
            _commandPublisher = commandPublisher;
            _hardwareBootstrap = hardwareBootstrap;
            _hubConnection = hubConnection;
            _commandBus = commandBus;

            commandSubscriber.Subscribe("Exit", OnCommandExit);
        }

        public async Task RunAsync()
        {
            ColoredConsole.WriteLineYellow("Robot application starting...");

            // Attach a handler for graceful shutdown
            Console.CancelKeyPress += Console_CancelKeyPress;

            var commandBusTask = StartCommandBus();
            await _commandPublisher.PublishAsync(ApplicationCommands.BeforeStart);
            _hardwareBootstrap.Initialize();
            await StartSignalRConnectionAsync();
            var controllerTasks = RunControllerHandlers();

            ColoredConsole.WriteLineGreen("Robot application started successfully.");

            await Task.WhenAll(controllerTasks.Append(commandBusTask));
            _hardwareBootstrap.Stop();

            ColoredConsole.WriteLineRed("Robot application has been stopped.");
        }

        private Task StartCommandBus()
        {
            ColoredConsole.WriteLineYellow("Starting Command Bus...");
            var task = _commandBus.ProcessCommandsAsync(_cancellationTokenSource.Token);
            ColoredConsole.WriteLineYellow("Command Bus was started");

            return task;
        }

        private async Task StartSignalRConnectionAsync()
        {
            ColoredConsole.WriteLineYellow("Starting SignalR connection...");
            await _hubConnection.StartAsync();
            ColoredConsole.WriteLineGreen("SignalR connection started successfully!");
        }

        private IEnumerable<Task> RunControllerHandlers()
        {
            ColoredConsole.WriteLineYellow("Running controller handlers...");
            return _controllers.Select(controller => controller.HandleAsync(_cancellationTokenSource.Token));
        }

        private void OnCommandExit(object? obj)
        {
            _cancellationTokenSource.Cancel();
        }

        private void Console_CancelKeyPress(object? sender, EventArgs e)
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                ColoredConsole.WriteLineYellow("\nProcess exit detected. Stopping hardware...");
                _cancellationTokenSource.Cancel();
                _hardwareBootstrap.Stop();
            }
        }
    }
}
