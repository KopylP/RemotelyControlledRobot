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

        public MoveController(IHardwareProvider provider, ICommandSubscriber commandSubscriber)
        {
            _driver = provider.GetRequiredHardware<IDriver>();

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