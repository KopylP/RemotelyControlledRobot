using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;
using RemotelyControlledRobot.IoT.Contracts;
using RemotelyControlledRobot.IoT.Contracts.Messages;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public class SignalRCommandController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        public SignalRCommandController(ICommandPublisher commandPublisher, HubConnection hubConnection)
        {
            _commandPublisher = commandPublisher;

            hubConnection.On<double, double>("ReceiveSpeedAndDirection", ReceiveSpeedAndDirection);
            hubConnection.On<int, int>("ReceiveCameraAngle", ReceiveCameraAngle);
        }

        private async Task ReceiveSpeedAndDirection(double speed, double direction)
        {
            ColoredConsole.WriteLineCyan($"Received speed: {speed}, direction: {direction} from SignalR.");
            await _commandPublisher.PublishAsync(MoveControllerCommands.SpeedWithDirection, SpeedWithDirectionMessage.Of(speed, direction));
        }

        private async Task ReceiveCameraAngle(int cameraAngleX, int cameraAngleY)
        {
            ColoredConsole.WriteLineCyan($"Received camera angle X: {cameraAngleX}, Y: {cameraAngleY} from SignalR.");
            await _commandPublisher.PublishAsync(CameraNeckControllerCommands.CameraAngle, new CameraAngleMessage(cameraAngleX, cameraAngleY));
        }
    }
}
