using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Contracts;
using RemotelyControlledRobot.IoT.Contracts.Messages;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Drivers;

namespace RemotelyControlledRobot.IoT.Application.Move.Controllers
{
    public sealed class MoveController : ControllerBase
    {
        private readonly Driver _driver;

        public MoveController(IHardwareProvider provider, ICommandSubscriber commandSubscriber)
        {
            _driver = provider.GetRequiredHardware<Driver>();

            commandSubscriber.Subscribe(MoveControllerCommands.Ahead, OnAhead);
            commandSubscriber.Subscribe(MoveControllerCommands.Stop, OnStop);
            commandSubscriber.Subscribe(MoveControllerCommands.Left, OnLeft);
            commandSubscriber.Subscribe(MoveControllerCommands.Right, OnRight);
            commandSubscriber.Subscribe(MoveControllerCommands.Back, OnBack);
            commandSubscriber.Subscribe(MoveControllerCommands.SpeedWithDirection, OnSpeedWithDirection);
        }

        private void OnSpeedWithDirection(object? message)
        {
            var speedWithDirectionMessage = (SpeedWithDirectionMessage)message!;
            _driver.SetSpeedWithDirection(speedWithDirectionMessage.Speed, speedWithDirectionMessage.Direction);
        }

        private void OnBack(object? _)
        {
            _driver.GoBack();
        }

        private void OnRight(object? _)
        {
            _driver.GoRight();
        }

        private void OnLeft(object? _)
        {
            _driver.GoLeft();
        }

        private void OnStop(object? _)
        {
            _driver.Stop();
        }

        private void OnAhead(object? _)
        {
            _driver.GoAhead();
        }
    }
}