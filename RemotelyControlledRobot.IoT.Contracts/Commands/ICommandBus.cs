namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandBus
	{
		void Subscribe(string commandType, Action<object?> messageCallback);
		void Unsubscribe(string commandType, Action<object?> messageCallback);
	}
}

