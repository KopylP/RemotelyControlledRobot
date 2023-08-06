using System.Collections.Concurrent;

namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandQueue
	{
		public event Action? OnEnqueue;
		public void Enqueue(string command, string message);
		public (string Command, string Message)? Dequeue();
	}
}

