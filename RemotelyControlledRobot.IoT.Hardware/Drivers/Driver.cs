using System.Device.Gpio;
using Iot.Device.DCMotor;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Hardware.Drivers
{
    public class Driver(DriverSettings settings) : IHardware
    {
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
            SetSpeedWithDirection(0.5, 0);
        }

        public void GoLeft()
        {
            SetSpeedWithDirection(0.5, -1);
        }

        public void GoRight()
        {
            SetSpeedWithDirection(0.5, 1);
        }

        public void Stop()
        {
            SetSpeedWithDirection(0, 0);
        }

        public void GoBack()
        {
            SetSpeedWithDirection(-0.5, 0);
        }

        public void SetSpeedWithDirection(double speed, double direction)
        {
            var motorSpeeds = DriverSpeedConverter
                .Convert(speed, direction, settings.CalibrationCoefficient);

            _leftMotor!.Speed = motorSpeeds.LeftMotorSpeed;
            _rightMotor!.Speed = motorSpeeds.RightMotorSpeed;
        }
    }
}

