using System;
using System.Threading.Channels;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public class CommandPublisher : ICommandPublisher
	{
        private readonly ChannelWriter<(string Command, object? Message)> _channelWriter;

        public CommandPublisher(ChannelWriter<(string Command, object? Message)> channelWriter)
        {
            _channelWriter = channelWriter;
        }

        public async Task PublishAsync(string command, object? message)
        {
            await _channelWriter.WriteAsync((command, message));
        }
    }
}

