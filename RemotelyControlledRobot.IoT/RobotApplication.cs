using System.Device.Gpio;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Core
{
	public class RobotApplication
	{
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly IMoveController _robotController;
        private readonly GpioController _gpioController;
        private readonly IInternalCommandPublisher _commandPublisher;

        public RobotApplication(IMoveController robotController, GpioController gpioController, IInternalCommandPublisher commandPublisher)
        {
            _robotController = robotController;
            _gpioController = gpioController;
            _commandPublisher = commandPublisher;
        }

        public async Task RunAsync()
		{
            ColoredConsole.WriteLineYellow("Running robot...");
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            _robotController.Initialize();
            await Task.Delay(3000);

            _commandPublisher.Publish(MoveControllerCommands.Ahead);
            _commandPublisher.Publish(MoveControllerCommands.Left);
            _commandPublisher.Publish(MoveControllerCommands.Right);

            ColoredConsole.WriteLineGreen("Robot was running successfully.");

            await Task.CompletedTask;
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            _gpioController.Dispose();
        }
	}
}