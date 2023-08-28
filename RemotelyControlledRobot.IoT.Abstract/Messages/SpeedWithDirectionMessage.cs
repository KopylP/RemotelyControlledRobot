using System;
namespace RemotelyControlledRobot.IoT.Abstract.Messages
{
	public record SpeedWithDirectionMessage
	{
		public double Speed { get; init; }
		public double Direction { get; init; }

		public static SpeedWithDirectionMessage Of(double speed, double direction) => new SpeedWithDirectionMessage
		{
			Speed = speed,
			Direction = direction,
		};
    }
}