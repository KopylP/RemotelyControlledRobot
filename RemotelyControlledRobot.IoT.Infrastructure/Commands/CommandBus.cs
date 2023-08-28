using System.Collections.Concurrent;
using System.Data;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandQueue _commandQueue;
        private ConcurrentDictionary<string, List<Action<object?>>> _commandSubscribers = new ConcurrentDictionary<string, List<Action<object?>>>();

        public CommandBus(ICommandQueue commandQueue)
        {
            _commandQueue = commandQueue;
            _commandQueue.OnEnqueue += CommandQueue_OnEnqueue;
        }

        private void CommandQueue_OnEnqueue()
        {
            var queuedCommand = _commandQueue.Dequeue();

            if (queuedCommand.HasValue)
            {
                var (command, message) = queuedCommand.Value;

                if (_commandSubscribers.ContainsKey(command))
                    _commandSubscribers[command].ForEach(action => action(message));
            }
        }

        public void Subscribe(string commandType, Action<object?> messageCallback)
        {
            messageCallback = messageCallback ?? throw new ArgumentNullException();

            if (!_commandSubscribers.ContainsKey(commandType))
            {
                _commandSubscribers[commandType] = new List<Action<object?>>();
            }

            if (!_commandSubscribers[commandType].Contains(messageCallback))
            {
                _commandSubscribers[commandType].Add(messageCallback);
            }
        }

        public void Unsubscribe(string commandType, Action<object?> messageCallback)
        {
            if (_commandSubscribers.ContainsKey(commandType))
            {
                if (_commandSubscribers[commandType].Contains(messageCallback))
                {
                    _commandSubscribers[commandType].Remove(messageCallback);
                }

                if (!_commandSubscribers[commandType].Any())
                {
                    _commandSubscribers.Remove(commandType, out _);
                }
            }
        }
    }
}

