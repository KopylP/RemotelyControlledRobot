using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Abstract.Messages;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public class SignalRCommandController : ControllerBase
    {
        private readonly HubConnection _hubConnection;
        private readonly ICommandPublisher _commandPublisher;

        public SignalRCommandController(ICommandPublisher commandPublisher, HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            _commandPublisher = commandPublisher;

            _hubConnection.On("ReceiveSpeedAndDirection", (double speed, double direction) =>
            {
                ColoredConsole.WriteLineCyan($"Received speed: {speed}, direction: {direction} from SignalR.");
                _commandPublisher.Publish(MoveControllerCommands.SpeedWithDirection, SpeedWithDirectionMessage.Of(speed, direction));
            });

            _hubConnection.On("ReceiveCameraAngle", (int cameraAngleX, int cameraAngleY) =>
            {
                ColoredConsole.WriteLineCyan($"Received camera angle X: {cameraAngleX}, Y: {cameraAngleY} from SignalR.");
                _commandPublisher.Publish(CameraNeckControllerCommands.CameraAngle, new CameraAngleMessage(cameraAngleX, cameraAngleY));
            });
        }
    }
}
