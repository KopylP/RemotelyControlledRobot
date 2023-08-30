namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandPublisher
	{
		Task PublishAsync(string command, object? message);

    }

	public static class IInternalCommandPublisherExtentions
	{
		public static async Task PublishAsync(this ICommandPublisher publisher, string command)
		{
			await publisher.PublishAsync(command, default);
		}

        public static void Publish(this ICommandPublisher publisher, string command, object? message)
        {
            _ = publisher.PublishAsync(command, message);
        }

        public static void Publish(this ICommandPublisher publisher, string command)
        {
            _ = publisher.PublishAsync(command);
        }
    }
}

