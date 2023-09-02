using System.Device.Gpio;
using Iot.Device.DCMotor;
using RemotelyControlledRobot.IoT.Contracts.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Drivers
{
    public class Driver : HardwareBase, IDriver
    {
        private readonly DriverSettings _settings;

        private DCMotor? _leftMotor;
        private DCMotor? _rightMotor;

        public Driver(DriverSettings settings) => _settings = settings;

        public override void Initialize(GpioController controller)
        {
            _leftMotor = DCMotor.Create(_settings.ENA, _settings.IN2, _settings.IN1, controller);
            _rightMotor = DCMotor.Create(_settings.ENB, _settings.IN4, _settings.IN3, controller);

            Stop();
        }

        public override void OnStop(GpioController gpioController)
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
                .Convert(speed, direction, _settings.CalibrationCoefficient);

            _leftMotor!.Speed = motorSpeeds.LeftMotorSpeed;
            _rightMotor!.Speed = motorSpeeds.RightMotorSpeed;
        }
    }
}

