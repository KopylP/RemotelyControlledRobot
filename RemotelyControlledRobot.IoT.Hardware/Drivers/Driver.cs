using System.Device.Gpio;
using Iot.Device.DCMotor;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Hardware.Drivers;

public class Driver(DriverSettings settings) : IHardware
{
    private const double DefaultSpeed = 0.5;
    private const double NeutralDirection = 0.0;
    private const double LeftTurnDirection = -1.0;
    private const double RightTurnDirection = 1.0;
    private const double StoppedSpeed = 0.0;

    private DCMotor? _leftMotor;
    private DCMotor? _rightMotor;

    public void Initialize(GpioController controller)
    {
        _leftMotor = DCMotor.Create(settings.ENA, settings.IN2, settings.IN1, controller);
        _rightMotor = DCMotor.Create(settings.ENB, settings.IN4, settings.IN3, controller);

        Stop();
    }

    public void Stop(GpioController gpioController)
        => Stop();

    public void GoAhead()
    {
        SetSpeedWithDirection(DefaultSpeed, NeutralDirection);
    }

    public void GoLeft()
    {
        SetSpeedWithDirection(DefaultSpeed, LeftTurnDirection);
    }

    public void GoRight()
    {
        SetSpeedWithDirection(DefaultSpeed, RightTurnDirection);
    }

    public void Stop()
    {
        SetSpeedWithDirection(StoppedSpeed, NeutralDirection);
    }

    public void GoBack()
    {
        SetSpeedWithDirection(-DefaultSpeed, NeutralDirection);
    }

    public void SetSpeedWithDirection(double speed, double direction)
    {
        var (leftMotorSpeed, rightMotorSpeed) = DriverSpeedConverter
                .Convert(speed, direction, settings.CalibrationCoefficient);

        _leftMotor!.Speed = leftMotorSpeed;
        _rightMotor!.Speed = rightMotorSpeed;
    }
}