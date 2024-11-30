using System.Threading.Channels;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal class CommandPublisher(ChannelWriter<(string Command, object? Message)> channelWriter)
    : ICommandPublisher
{
    public async Task PublishAsync(string command, object? message)
    {
        await channelWriter.WriteAsync((command, message));
    }
}