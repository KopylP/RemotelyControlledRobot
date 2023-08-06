namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface IInternalCommandPublisher
	{
		void Publish(string command, string message);
	}

	public static class IInternalCommandPublisherExtentions
	{
		public static void Publish(this IInternalCommandPublisher publisher, string command)
		{
			publisher.Publish(command, string.Empty);
		}
	}
}

