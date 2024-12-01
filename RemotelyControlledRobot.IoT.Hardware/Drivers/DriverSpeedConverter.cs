namespace RemotelyControlledRobot.IoT.Hardware.Drivers
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
        /// <param name="calibrationCoefficient">A coefficient indicating the calibration factor of the right motor compared to the left motor.
        /// Values greater than 1 make the right motor weaker, and values less than 1 make the right motor stronger.</param>
        /// <returns>A tuple containing the left and right motor speeds.</returns>
        public static (double LeftMotorSpeed, double RightMotorSpeed) Convert(double speed, double direction, double calibrationCoefficient = 1)
		{
            var rightMotorCoefficient = direction > 0 ? 1 - direction : 1;
            var leftMotorCoefficient = direction < 0 ? 1 + direction : 1;

            leftMotorCoefficient = leftMotorCoefficient * calibrationCoefficient;

            return (leftMotorCoefficient * speed, rightMotorCoefficient * speed);
        }
	}
}