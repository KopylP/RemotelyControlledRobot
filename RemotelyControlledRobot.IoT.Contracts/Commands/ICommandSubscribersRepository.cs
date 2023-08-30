using System;
namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandSubscribersRepository
	{
        IReadOnlyList<Action<object?>> Get(string command);
        void Add(string command, Action<object?> messageCallback);
        void Remove(string command, Action<object?> messageCallback);
    }
}

