using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public class CommandsSubscriber : ICommandSubscriber
	{
        private readonly ICommandSubscribersRepository _subscribersRepository;

        public CommandsSubscriber(ICommandSubscribersRepository subscribersRepository)
        {
            _subscribersRepository = subscribersRepository;
        }


        public void Subscribe(string commandType, Action<object?> messageCallback)
        {
            _subscribersRepository.Add(commandType, messageCallback);
        }

        public void Unsubscribe(string commandType, Action<object?> messageCallback)
        {
            _subscribersRepository.Remove(commandType, messageCallback);
        }
    }
}

