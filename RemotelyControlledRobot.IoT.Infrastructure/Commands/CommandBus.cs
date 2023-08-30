using System.Threading.Channels;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.Framework.Extensions;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly ChannelReader<(string Command, object? Message)> _channelReader;
        private readonly ICommandSubscribersRepository _subscribersRepository;

        public CommandBus(
            ChannelReader<(string Command, object? Message)> channelReader,
            ICommandSubscribersRepository subscribersRepository)
        {
            _channelReader = channelReader;
            _subscribersRepository = subscribersRepository;
        }

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
            await foreach (var command in _channelReader.ReadAllAsync(cancellationToken))
            {
                InvokeCommandSubscribersActions(command);
            }
        }

        private void InvokeCommandSubscribersActions((string Command, object? Message) command)
        {
            var actions = _subscribersRepository.Get(command.Command);
            actions.ForEach(action => action(command.Message));
        }
    }
}

