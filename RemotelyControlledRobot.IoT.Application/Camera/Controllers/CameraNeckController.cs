using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Contracts;
using RemotelyControlledRobot.IoT.Contracts.Messages;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Cameras;

namespace RemotelyControlledRobot.IoT.Application.Camera.Controllers;

public class CameraNeckController : ControllerBase
{
    private readonly CameraNeck _cameraNeck;

    public CameraNeckController(IHardwareProvider provider, ICommandSubscriber commandSubscriber)
    {
        _cameraNeck = provider.GetRequiredHardware<CameraNeck>();
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