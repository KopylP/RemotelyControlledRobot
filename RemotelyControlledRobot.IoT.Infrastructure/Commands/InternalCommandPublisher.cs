using System;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public class InternalCommandPublisher : IInternalCommandPublisher
	{
        private readonly ICommandQueue _memoryQueue;

        public InternalCommandPublisher(ICommandQueue memoryQueue)
        {
            _memoryQueue = memoryQueue;
                    }

        void IInternalCommandPublisher.Publish(string command, string message)
        {
            _memoryQueue.Enqueue(command, message);
        }
    }
}

