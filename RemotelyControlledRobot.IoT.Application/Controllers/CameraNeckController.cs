using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Abstract.Messages;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public class CameraNeckController : ControllerBase
    {
        private readonly ICameraNeck _cameraNeck;

        public CameraNeckController(IHardwareProvider provider, ICommandSubscriber commandSubscriber)
        {
            _cameraNeck = provider.GetRequiredHardware<ICameraNeck>();
            commandSubscriber.Subscribe(CameraNeckControllerCommands.CameraLeft, OnCameraLeft);
            commandSubscriber.Subscribe(CameraNeckControllerCommands.CameraRight, OnCameraRight);
            commandSubscriber.Subscribe(CameraNeckControllerCommands.CameraAhead, OnCameraAhead);
            commandSubscriber.Subscribe(CameraNeckControllerCommands.CameraAngle, OnCameraAngle);
        }

        private void OnCameraAngle(object? message)
        {
            var cameraAngleMessage = (CameraAngleMessage)message!;
            _cameraNeck.WriteXAngle(cameraAngleMessage.CameraAngleX);
            _cameraNeck.WriteYAngle(cameraAngleMessage.CameraAngleY);
        }

        private void OnCameraAhead(object? _)
        {
            _cameraNeck.TurnAhead();
        }

        private void OnCameraRight(object? _)
        {
            _cameraNeck.TurnRightMax();
        }

        private void OnCameraLeft(object? _)
        {
            _cameraNeck.TurnLeftMax();
        }
    }
}
