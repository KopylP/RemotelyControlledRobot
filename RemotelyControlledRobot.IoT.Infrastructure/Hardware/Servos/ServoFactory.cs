using System.Device.Pwm.Drivers;
using Iot.Device.ServoMotor;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Servos;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Servos
{
    public class ServoFactory : IServoFactory
    {
        public ServoFactory()
        {
        }

        public ServoMotor CreateSoftwareSW90Servo(int pinNumber)
        {
            var pwmChannel = new SoftwarePwmChannel(pinNumber, frequency: 50, usePrecisionTimer: true);
            var servo = new ServoMotor(pwmChannel, maximumAngle: 180, minimumPulseWidthMicroseconds: 500, maximumPulseWidthMicroseconds: 2500);
            servo.WriteAngle(2);

            return servo;
        }
    }
}

