using System;
using Iot.Device.ServoMotor;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Servos
{
	public class AutoDisabledServo : IDisposable
	{
        private bool _disposed = false;

        private readonly int _delayInMillisecondsBeforeStopServo;

        private bool _servoIsStarted;

        private readonly ServoMotor _servo;
        private readonly Timer _disableServoTimer;

        private int _previousAngle = int.MinValue;
        private readonly bool _reverseAngle;

        public AutoDisabledServo(ServoMotor servo, int delayInMilliseconds = 500, bool reverseAngle = false)
		{
			_servo = servo;
            _delayInMillisecondsBeforeStopServo = delayInMilliseconds;
            _disableServoTimer = new Timer(DisableServo, null, Timeout.Infinite, Timeout.Infinite);
            _reverseAngle = reverseAngle;
        }

        public void WriteAngle(int angle)
        {
            if (_previousAngle != angle)
            {
                _previousAngle = angle;

                if (_reverseAngle)
                    angle = 180 - angle;

                StartServo();
                _servo?.WriteAngle(angle);
                _disableServoTimer?.Change(_delayInMillisecondsBeforeStopServo, Timeout.Infinite);
            }
        }

        private void DisableServo(object? state)
        {
            if (_servoIsStarted)
            {
                _disableServoTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                _servo?.Stop();
                _servoIsStarted = false;
            }
        }

        private void StartServo()
        {
            if (!_servoIsStarted)
            {
                _servo?.Start();
                _servoIsStarted = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _servo.Stop();
            _servo.Dispose();

            _disposed = true;
        }

        ~AutoDisabledServo()
        {
            Dispose(false);
        }
    }
}

