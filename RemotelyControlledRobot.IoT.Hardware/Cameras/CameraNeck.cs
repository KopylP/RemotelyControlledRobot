using System.Device.Gpio;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Hardware.Servos;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Hardware.Cameras;

public class CameraNeck(CameraNeckSettings settings) : IHardware
{
    private AutoDisabledServo? _xServo;
    private AutoDisabledServo? _yServo;

    public void Initialize(GpioController gpioController)
    {
        _xServo = CreateServo(settings.LeftRightServoPin);
        _yServo = CreateServo(settings.UpDownServoPin);
        InitializeServoAngles();
    }

    public void TurnLeftMax()
    {
        WriteXAngle(angle: 0);
    }

    public void TurnAhead()
    {
        WriteXAngle(angle: 90);
    }

    public void TurnRightMax()
    {
        WriteXAngle(angle: 180);
    }

    public void WriteXAngle(int angle) => _xServo?.WriteAngle(angle);
    public void WriteYAngle(int angle) => _yServo?.WriteAngle(angle);

    public void Stop(GpioController gpioController)
    {
        _xServo?.Dispose();
        _yServo?.Dispose();
    }

    private AutoDisabledServo CreateServo(int servoPin)
    {
        return new AutoDisabledServo(
            ServoFactory.CreateSoftwareSw90Servo(servoPin),
            reverseAngle: settings.ReverseLeftRightServoAngle);
    }

    private void InitializeServoAngles()
    {
        WriteXAngle(settings.LeftRightServoDefaultAngle);
        WriteYAngle(settings.UpDownServoDefaultAngle);
    }
}