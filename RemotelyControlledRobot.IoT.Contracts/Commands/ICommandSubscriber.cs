using System;
namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandSubscriber
	{
        void Subscribe(string commandType, Action<object?> messageCallback);
        void Unsubscribe(string commandType, Action<object?> messageCallback);
    }
}

