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
            await Task.Run(() =>
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
                            _commandPublisher.Publish(MoveControllerCommands.Left);
                            _lastMoveCommand = MoveControllerCommands.Left;
                            break;

                        case ConsoleKey.D:
                            ColoredConsole.WriteLineCyan("\nSent command: Move right");
                            _commandPublisher.Publish(MoveControllerCommands.Right);
                            _lastMoveCommand = MoveControllerCommands.Right;
                            break;

                        case ConsoleKey.S:
                            if (_lastMoveCommand == MoveControllerCommands.Stop)
                            {
                                ColoredConsole.WriteLineCyan("\nSent command: Move back");
                                _commandPublisher.Publish(MoveControllerCommands.Back);
                                _lastMoveCommand = MoveControllerCommands.Back;
                                break;
                            }

                            ColoredConsole.WriteLineCyan("\nSent command: Stop");
                            _commandPublisher.Publish(MoveControllerCommands.Stop);
                            _lastMoveCommand = MoveControllerCommands.Stop;
                            break;

                        case ConsoleKey.W:
                            if (_lastMoveCommand == MoveControllerCommands.Back)
                            {
                                ColoredConsole.WriteLineCyan("\nSent command: Stop");
                                _commandPublisher.Publish(MoveControllerCommands.Stop);
                                _lastMoveCommand = MoveControllerCommands.Stop;
                                break;
                            }

                            ColoredConsole.WriteLineCyan("\nSent command: Move ahead");
                            _commandPublisher.Publish(MoveControllerCommands.Ahead);
                            _lastMoveCommand = MoveControllerCommands.Ahead;
                            break;

                        case ConsoleKey.LeftArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera left");
                            _commandPublisher.Publish(CameraNeckControllerCommands.CameraLeft);
                            break;

                        case ConsoleKey.RightArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera right");
                            _commandPublisher.Publish(CameraNeckControllerCommands.CameraRight);
                            break;

                        case ConsoleKey.UpArrow:
                            ColoredConsole.WriteLineCyan("\nSent command: Move camera ahead");
                            _commandPublisher.Publish(CameraNeckControllerCommands.CameraAhead);
                            break;

                        case ConsoleKey.Q:
                            ColoredConsole.WriteLineCyan("\nSent command: Exit");
                            _commandPublisher.Publish("Exit");
                            return;

                        default:
                            break;
                    }
                }
            });
        }
    }
}
