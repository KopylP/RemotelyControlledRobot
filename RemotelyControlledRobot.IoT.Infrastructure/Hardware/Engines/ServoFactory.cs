using System;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using Iot.Device.ServoMotor;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Engines;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Engines
{
    public class ServoFactory : IServoFactory
    {
        public ServoFactory()
        {
        }

        public ServoMotor AttachSW90Servo(int pinNumber)
        {
            var pwmChannel = new SoftwarePwmChannel(pinNumber, frequency: 50, usePrecisionTimer: true);
            var servo = new ServoMotor(pwmChannel, maximumAngle: 180, minimumPulseWidthMicroseconds: 500, maximumPulseWidthMicroseconds: 2500);
            servo.WriteAngle(2);

            return servo;
        }
    }
}

