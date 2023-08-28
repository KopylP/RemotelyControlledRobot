using System;
namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Drivers
{
	public static class DriverSpeedConverter
	{
        /// <summary>
        /// Converts a speed and direction into left and right motor speeds.
        /// </summary>
        /// <param name="speed">Speed value ranging from -1 to 1:
        /// -1 to 0 for moving backward, 0 to 1 for moving forward.</param>
        /// <param name="direction">Direction value ranging from -1 to 1:
        /// -1 to 0 for turning left, 0 to 1 for turning right.</param>
        /// <returns>A tuple containing the left and right motor speeds.</returns>
        public static (double LeftMotorSpeed, double RightMotorSpeed) Convert(double speed, double direction)
		{
            var rightMotorCoeficient = direction > 0 ? 1 - direction : 1;
            var leftMotorCoeficient = direction < 0 ? 1 + direction : 1;

            return (leftMotorCoeficient * speed, rightMotorCoeficient * speed);
        }
	}
}