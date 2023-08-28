using System.Device.Gpio;
using Iot.Device.DCMotor;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Drivers
{
    public class Driver : HardwareBase, IDriver
    {
        private DCMotor? _leftMotor;
        private DCMotor? _rightMotor;

        private const int IN1 = 12;
        private const int IN2 = 13;
        private const int ENA = 6;

        private const int IN4 = 20;
        private const int IN3 = 21;
        private const int ENB = 26;

        public override void Initialize(GpioController controller)
        {
            _leftMotor = DCMotor.Create(ENA, IN2, IN1, controller);
            _rightMotor = DCMotor.Create(ENB, IN4, IN3, controller);

            Stop();
        }

        public override void OnStop(GpioController gpioController)
            => Stop();


        public void GoAhead()
        {
            _leftMotor!.Speed = 0.5;
            _rightMotor!.Speed = 0.5;
        }

        public void GoLeft()
        {
            _leftMotor!.Speed = 0;
            _rightMotor!.Speed = 0.5;
        }

        public void GoRight()
        {
            _leftMotor!.Speed = 0.5;
            _rightMotor!.Speed = 0;
        }

        public void SetSpeedWithDirection(double speed, double direction)
        {
            var motorSpeeds = DriverSpeedConverter.Convert(speed, direction);

            _leftMotor!.Speed = motorSpeeds.LeftMotorSpeed;
            _rightMotor!.Speed = motorSpeeds.RightMotorSpeed;
        }

        public void Stop()
        {
            _leftMotor!.Speed = 0;
            _rightMotor!.Speed = 0;
        }

        public void GoBack()
        {
            _leftMotor!.Speed = -0.5;
            _rightMotor!.Speed = -0.5;
        }
    }
}

