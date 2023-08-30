﻿using System;
using RemotelyControlledRobot.Framework;
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
            ColoredConsole.WriteLineCyan($"Camera angle changed. X: {cameraAngleMessage.CameraAngleX}, Y: {cameraAngleMessage.CameraAngleY}");
        }

        private void OnCameraAhead(object? _)
        {
            ColoredConsole.WriteLineYellow("Camera moving ahead");
            _cameraNeck.TurnAhead();
        }

        private void OnCameraRight(object? _)
        {
            ColoredConsole.WriteLineYellow("Camera turning right");
            _cameraNeck.TurnRightMax();
        }

        private void OnCameraLeft(object? _)
        {
            ColoredConsole.WriteLineYellow("Camera turning left");
            _cameraNeck.TurnLeftMax();
        }
    }
}
