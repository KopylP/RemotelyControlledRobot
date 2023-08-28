using System;
using Iot.Device.ServoMotor;

namespace RemotelyControlledRobot.IoT.Contracts.Hardware.Engines
{
    public interface IServoFactory
    {
        public ServoMotor AttachSW90Servo(int pinNumber);
    }
}

