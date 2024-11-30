using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;
using RemotelyControlledRobot.IoT.Contracts;

namespace RemotelyControlledRobot.IoT.Application.Main.Controllers
{
    [ControllerEnable(ConfigurationKeys.ConsoleControllerEnabled)]
    public class ConsoleController(ICommandPublisher commandPublisher) : ControllerBase, IController
    {
        private string _lastMoveCommand = string.Empty;
        private bool _exitDetected = false;

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            ColoredConsole.WriteLineGreen("Console controller started. Press 'Q' to exit.");

            while (!cancellationToken.IsCancellationRequested && !_exitDetected)
            {
                await ProcessConsoleInputAsync();
            }
        }

        private async Task ProcessConsoleInputAsync()
        {
            Console.Write("\nPlease, provide command key: ");
            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.A:
                    await SendMoveCommand(MoveControllerCommands.Left);
                    break;

                case ConsoleKey.D:
                    await SendMoveCommand(MoveControllerCommands.Right);
                    break;

                case ConsoleKey.S:
                    await SendMoveCommand(MoveControllerCommands.Stop, MoveControllerCommands.Back);
                    break;

                case ConsoleKey.W:
                    await SendMoveCommand(MoveControllerCommands.Ahead, MoveControllerCommands.Stop);
                    break;

                case ConsoleKey.LeftArrow:
                    await SendCameraCommand(CameraNeckControllerCommands.CameraLeft);
                    break;

                case ConsoleKey.RightArrow:
                    await SendCameraCommand(CameraNeckControllerCommands.CameraRight);
                    break;

                case ConsoleKey.UpArrow:
                    await SendCameraCommand(CameraNeckControllerCommands.CameraAhead);
                    break;

                case ConsoleKey.Q:
                    await Exit();
                    break;

                default:
                    break;
            }
        }

        private async Task SendMoveCommand(string newCommand, string rebaseCommand = MoveControllerCommands.Stop)
        {
            if (_lastMoveCommand == newCommand)
            {
                newCommand = rebaseCommand;
            }

            ColoredConsole.WriteLineCyan($"\nSent command: {newCommand}");
            await commandPublisher.PublishAsync(newCommand);
            _lastMoveCommand = newCommand;
        }

        private async Task SendCameraCommand(string command)
        {
            ColoredConsole.WriteLineCyan($"\nSent command: {command}");
            await commandPublisher.PublishAsync(command);
        }

        private async Task Exit()
        {
            ColoredConsole.WriteLineCyan("\nSent command: Exit");
            await commandPublisher.PublishAsync("Exit");
            _exitDetected = true;
        }
    }
}
