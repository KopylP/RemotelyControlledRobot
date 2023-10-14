using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Core
{
    public class RobotApplication
    {
        private CancellationTokenSource _controllerHandlersCancellationTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _commandBusCancellationTokenSource = new CancellationTokenSource();

        private readonly IEnumerable<IController> _controllers;
        private readonly IHardwareBootstrapper _hardwareBootstrap;
        private readonly HubConnection _hubConnection;
        private readonly ICommandBus _commandBus;

        public RobotApplication(
            IEnumerable<IController> controllers,
            IHardwareBootstrapper hardwareBootstrap,
            HubConnection hubConnection,
            ICommandSubscriber commandSubscriber,
            ICommandBus commandBus)
        {
            _controllers = controllers;
            _hardwareBootstrap = hardwareBootstrap;
            _hubConnection = hubConnection;
            _commandBus = commandBus;

            commandSubscriber.Subscribe("Exit", OnCommandExit);
        }

        public async Task RunAsync()
        {
            InitializeApplication();
            BootstrapHardware();
            StartCommandBus();
            await StartSignalRConnectionAsync();
            await RunAndWaitForControllerHandlersAsync();
            StopRobotApplication();
        }

        private void InitializeApplication()
        {
            ColoredConsole.WriteLineYellow("Robot application starting...");
            Console.CancelKeyPress += Console_CancelKeyPress;
        }

        private void BootstrapHardware()
        {
            _hardwareBootstrap.Initialize();
        }

        private void StartCommandBus()
        {
            ColoredConsole.WriteLineYellow("Starting Command Bus...");
            _ = _commandBus.ProcessCommandsAsync(_commandBusCancellationTokenSource.Token);
            ColoredConsole.WriteLineGreen("Command Bus was started");
        }

        private async Task StartSignalRConnectionAsync()
        {
            ColoredConsole.WriteLineYellow("Starting SignalR connection...");
            await _hubConnection.StartAsync();
            ColoredConsole.WriteLineGreen("SignalR connection started successfully!");
        }

        private async Task RunAndWaitForControllerHandlersAsync()
        {
            ColoredConsole.WriteLineYellow("Running controller handlers...");
            var tasks = _controllers
                .Select(controller => controller
                .HandleAsync(_controllerHandlersCancellationTokenSource.Token));
            ColoredConsole.WriteLineGreen("Robot application started successfully.");

            await Task.WhenAll(tasks);
        }

        private void StopRobotApplication()
        {
            _hardwareBootstrap.Stop();
            _commandBusCancellationTokenSource.Cancel();
            ColoredConsole.WriteLineRed("Robot application has been stopped.");
        }

        private void OnCommandExit(object? obj)
        {
            _controllerHandlersCancellationTokenSource.Cancel();
        }

        private void Console_CancelKeyPress(object? sender, EventArgs e)
        {
            if (!_controllerHandlersCancellationTokenSource.IsCancellationRequested)
            {
                ColoredConsole.WriteLineRed("\nProcess exit detected. Stopping hardware...");
                _controllerHandlersCancellationTokenSource.Cancel();
                _hardwareBootstrap.Stop();
                _commandBusCancellationTokenSource.Cancel();
            }
        }
    }
}
