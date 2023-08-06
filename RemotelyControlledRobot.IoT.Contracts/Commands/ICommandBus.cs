namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandBus
	{
		void Subscribe(string commandType, Action<string> messageCallback);
		void Unsubscribe(string commandType, Action<string> messageCallback);
	}
}

