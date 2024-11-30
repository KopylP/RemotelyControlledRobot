using System.Threading.Channels;
using RemotelyControlledRobot.Framework.Extensions;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal class CommandBus(
    ChannelReader<(string Command, object? Message)> channelReader,
    CommandSubscribersRepository subscribersRepository)
{
    public async Task ProcessCommandsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ProcessCommandsStreamAsync(cancellationToken);
        }
        catch(TaskCanceledException)
        {
            ColoredConsole.WriteLineRed("Commands processing was stopped.");
        }
    }

    private async Task ProcessCommandsStreamAsync(CancellationToken cancellationToken)
    {
        await foreach (var command in channelReader.ReadAllAsync(cancellationToken))
        {
            InvokeCommandSubscribersActions(command);
        }
    }

    private void InvokeCommandSubscribersActions((string Command, object? Message) command)
    {
        var actions = subscribersRepository.Get(command.Command);
        actions.ForEach(action => action(command.Message));
    }
}