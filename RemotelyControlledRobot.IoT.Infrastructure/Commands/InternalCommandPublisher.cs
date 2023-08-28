using System;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public class InternalCommandPublisher : ICommandPublisher
	{
        private readonly ICommandQueue _memoryQueue;

        public InternalCommandPublisher(ICommandQueue memoryQueue)
        {
            _memoryQueue = memoryQueue;
                    }

        void ICommandPublisher.Publish(string command, object? message)
        {
            _memoryQueue.Enqueue(command, message);
        }
    }
}

