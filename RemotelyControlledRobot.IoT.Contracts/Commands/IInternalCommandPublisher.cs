namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandPublisher
	{
		void Publish(string command, object? message);
	}

	public static class IInternalCommandPublisherExtentions
	{
		public static void Publish(this ICommandPublisher publisher, string command)
		{
			publisher.Publish(command, default);
		}
	}
}

