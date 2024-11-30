using System.Device.Pwm.Drivers;
using Iot.Device.ServoMotor;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Servos;

public static class ServoFactory
{
    public static ServoMotor CreateSoftwareSw90Servo(int pinNumber)
    {
        var pwmChannel = new SoftwarePwmChannel(pinNumber, frequency: 50, usePrecisionTimer: true);
        var servo = new ServoMotor(pwmChannel, maximumAngle: 180, minimumPulseWidthMicroseconds: 500, maximumPulseWidthMicroseconds: 2500);
        servo.WriteAngle(2);

        return servo;
    }
}