using System;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public class ConsoleController : ControllerBase, IController
    {
        private readonly ICommandPublisher _commandPublisher;
        private string _lastMoveCommand = string.Empty;

        public ConsoleController(ICommandPublisher commandPublisher) => _commandPublisher = commandPublisher;

        public async override Task HandleAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                ColoredConsole.WriteLineGreen("Console controller started. Press 'Q' to exit.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.Write("\nPlease, provide command key: ");
                    var key = Console.ReadKey();

                    switch (key.Key)
                    {
                        case ConsoleKey.A:
                            ColoredConsole.WriteLineCyan("\nSent command: Move left");
                            await _commandPublisher.PublishAsync(MoveControllerCommands.Left);
                            _lastMoveCommand = MoveControllerCommands.Left;
                            break;

                        case ConsoleKey.D:
                            ColoredConsole.WriteLineCyan("\nSent command: Move right");
                            await _commandPublisher.PublishAsync(MoveControllerCommands.Right);
                            _lastMoveCommand = MoveControllerCommands.Right;
                            break;

                        case ConsoleKey.S:
                            if (_lastMoveCommand == MoveControllerCommands.Stop)
                            {
                                ColoredConsole.WriteLineCyan("\nSent command: Move back");
                                await _commandPublisher.PublishAsync(MoveControllerCommands.Back);
                                _lastMoveCommand = MoveControllerCommands.Back;
                                break;
                            }

                            ColoredConsole.WriteLineCyan("\nSent command: Stop");
                            await _commandPublisher.PublishAsync(MoveControllerCommands.Stop);
                            _lastMoveCommand = MoveControllerCommands.Stop;
                            break;

                        case ConsoleKey.W:
                            if (_lastMoveCommand == MoveControllerCommands.Back)
                            {
                                ColoredConsole.WriteLineCyan("\nSent command: Stop");
                                await _commandPublisher.PublishAsync(MoveControllerCommands.Stop);
                                _lastMoveCommand = MoveControllerCommands.Stop;
                                break;
                            }

                            ColoredConsole.WriteLineCyan("\nSent command: Move ahead");
                            await _commandPublisher.PublishAsync(MoveControllerCommands.Ahead);
                            _lastMoveCommand = MoveControllerCommands.Ahead;
                            break;

                        case ConsoleKey.LeftArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera left");
                            await _commandPublisher.PublishAsync(CameraNeckControllerCommands.CameraLeft);
                            break;

                        case ConsoleKey.RightArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera right");
                            await _commandPublisher.PublishAsync(CameraNeckControllerCommands.CameraRight);
                            break;

                        case ConsoleKey.UpArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera ahead");
                            await _commandPublisher.PublishAsync(CameraNeckControllerCommands.CameraAhead);
                            break;

                        case ConsoleKey.Q:
                            ColoredConsole.WriteLineCyan("\nSent command: Exit");
                            await _commandPublisher.PublishAsync("Exit");
                            return;

                        default:
                            break;
                    }
                }
            });
        }
    }
}
