using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;
using RemotelyControlledRobot.IoT.Application.SignalR;
using RemotelyControlledRobot.IoT.Contracts;
using RemotelyControlledRobot.IoT.Contracts.Messages;

namespace RemotelyControlledRobot.IoT.Infrastructure.SignalR;

public class SignalRBootstrapper(
    ISignalRClient signalRClient,
    ICommandPublisher commandPublisher) : IStartLifecycle
{
    public async Task OnStartAsync(CancellationToken cancellationToken)
    {
        ColoredConsole.WriteLineYellow("Starting SignalR connection...");
        await signalRClient.ConnectAsync();
        ColoredConsole.WriteLineGreen("SignalR connection started successfully!");

        await signalRClient.SubscribeToEventAsync<double, double>("ReceiveSpeedAndDirection", ReceiveSpeedAndDirection);
        await signalRClient.SubscribeToEventAsync<int, int>("ReceiveCameraAngle", ReceiveCameraAngle);
    }

    private async Task ReceiveSpeedAndDirection(double speed, double direction)
    {
        ColoredConsole.WriteLineCyan($"Received speed: {speed}, direction: {direction} from SignalR.");
        await commandPublisher.PublishAsync(MoveControllerCommands.SpeedWithDirection, SpeedWithDirectionMessage.Of(speed, direction));
    }

    private async Task ReceiveCameraAngle(int cameraAngleX, int cameraAngleY)
    {
        ColoredConsole.WriteLineCyan($"Received camera angle X: {cameraAngleX}, Y: {cameraAngleY} from SignalR.");
        await commandPublisher.PublishAsync(CameraNeckControllerCommands.CameraAngle, new CameraAngleMessage(cameraAngleX, cameraAngleY));
    }
}