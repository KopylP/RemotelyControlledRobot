using System.Device.Gpio;
using RemotelyControlledRobot.IoT.Contracts.Hardware;
using RemotelyControlledRobot.IoT.Contracts.Hardware.Engines;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Servos;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware
{
    public class CameraNeck : HardwareBase, ICameraNeck
	{
        private readonly CameraNeckSettings _settings;

        private AutoDisabledServo? _xServo;
        private AutoDisabledServo? _yServo;

        private readonly IServoFactory _servoFacotry;

        public CameraNeck(IServoFactory servoFactory, CameraNeckSettings settings)
		{
            _servoFacotry = servoFactory;
            _settings = settings;
        }

        public override void Initialize(GpioController gpioController)
        {
            _xServo = new AutoDisabledServo(
                _servoFacotry.AttachSW90Servo(_settings.LeftRightServoPin),
                reverseAngle: _settings.ReverseLeftRightServoAngle);
            _yServo = new AutoDisabledServo(
                _servoFacotry.AttachSW90Servo(_settings.UpDownServoPin),
                reverseAngle: _settings.ReverseUpDownServoAngle);

            WriteXAngle(90);
            WriteYAngle(25);
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
    }
}
