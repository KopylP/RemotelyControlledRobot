using System;
using System.Collections.Concurrent;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public class MemoryCommandQueue : ICommandQueue
	{
        private readonly ConcurrentQueue<(string Command, object? Message)> _messages = new ConcurrentQueue<(string Command, object? Message)>();

        public event Action? OnEnqueue;

        public (string Command, object? Message)? Dequeue()
        {
            if (_messages.TryDequeue(out var result))
            {
                return result;
            }

            return null;
        }

        public void Enqueue(string command, object? message)
        {
            _messages.Enqueue((command, message));
            OnEnqueue?.Invoke();
        }
    }
}

