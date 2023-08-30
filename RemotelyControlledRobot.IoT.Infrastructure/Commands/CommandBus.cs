using System.Collections.Concurrent;
using System.Data;
using System.Threading.Channels;
using RemotelyControlledRobot.Framework.Extentions;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly ChannelReader<(string Command, object? Message)> _channelReader;
        private readonly ICommandSubscribersRepository _subscribersRepository;

        public CommandBus(ChannelReader<(string Command, object? Message)> channelReader, ICommandSubscribersRepository subscribersRepository)
        {
            _channelReader = channelReader;
            _subscribersRepository = subscribersRepository;
        }

        public async Task ProcessCommandsAsync(CancellationToken cancellationToken)
        {
            await foreach (var command in _channelReader.ReadAllAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var actions = _subscribersRepository.Get(command.Command);
                actions.ForEach(action => action(command.Message));
            }
        }
    }
}

