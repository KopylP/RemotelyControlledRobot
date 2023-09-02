using System.Device.Gpio;
using RemotelyControlledRobot.IoT.Contracts.Hardware;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Servos;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Servos;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware
{
    public class CameraNeck : HardwareBase, ICameraNeck
	{
        private readonly CameraNeckSettings _settings;

        private AutoDisabledServo? _xServo;
        private AutoDisabledServo? _yServo;

        private readonly IServoFactory _servoFactory;

        public CameraNeck(IServoFactory servoFactory, CameraNeckSettings settings)
		{
            _servoFactory = servoFactory;
            _settings = settings;
        }

        public override void Initialize(GpioController gpioController)
        {
            _xServo = CreateServo(_settings.LeftRightServoPin);
            _yServo = CreateServo(_settings.UpDownServoPin);
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

        public override void OnStop(GpioController gpioController)
        {
            _xServo?.Dispose();
            _yServo?.Dispose();
        }

        private AutoDisabledServo CreateServo(int servoPin)
        {
            return new AutoDisabledServo(
                _servoFactory.CreateSoftwareSW90Servo(servoPin),
                reverseAngle: _settings.ReverseLeftRightServoAngle);
        }

        private void InitializeServoAngles()
        {
            WriteXAngle(_settings.LeftRightServoDefaultAngle);
            WriteYAngle(_settings.UpDownServoDefaultAngle);
        }
    }
}
