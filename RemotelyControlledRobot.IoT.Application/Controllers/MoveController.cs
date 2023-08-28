using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Abstract.Messages;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public sealed class MoveController : ControllerBase
    {
        private readonly IDriver _driver;

        public MoveController(IHardwareProvider provider, ICommandBus commandBus)
        {
            _driver = provider.GetRequiredHardware<IDriver>();

            commandBus.Subscribe(MoveControllerCommands.Ahead, OnAhead);
            commandBus.Subscribe(MoveControllerCommands.Stop, OnStop);
            commandBus.Subscribe(MoveControllerCommands.Left, OnLeft);
            commandBus.Subscribe(MoveControllerCommands.Right, OnRight);
            commandBus.Subscribe(MoveControllerCommands.Back, OnBack);
            commandBus.Subscribe(MoveControllerCommands.SpeedWithDirection, OnSpeedWithDirection);
        }

        private void OnSpeedWithDirection(object? message)
        {
            var speedWithDirectionMessage = (SpeedWithDirectionMessage)message!;
            _driver.SetSpeedWithDirection(speedWithDirectionMessage.Speed, speedWithDirectionMessage.Direction);
            Console.WriteLine($"Changed speed to {speedWithDirectionMessage.Speed}, direction to {speedWithDirectionMessage.Direction}");
        }

        private void OnBack(object? _)
        {
            Console.WriteLine("Moving back");
            _driver.GoBack();
        }

        private void OnRight(object? _)
        {
            Console.WriteLine("Turning right");
            _driver.GoRight();
        }

        private void OnLeft(object? _)
        {
            Console.WriteLine("Turning left");
            _driver.GoLeft();
        }

        private void OnStop(object? _)
        {
            Console.WriteLine("Stopping");
            _driver.Stop();
        }

        private void OnAhead(object? _)
        {
            ColoredConsole.WriteLineYellow("Moving ahead");
            _driver.GoAhead();
        }
    }
}