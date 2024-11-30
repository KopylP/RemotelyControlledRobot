using RemotelyControlledRobot.Framework.System.Abstractions.Bus;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal class CommandsSubscriber(CommandSubscribersRepository subscribersRepository) : ICommandSubscriber
{
    public void Subscribe(string commandType, Action<object?> messageCallback)
    {
        subscribersRepository.Add(commandType, messageCallback);
    }

    public void Unsubscribe(string commandType, Action<object?> messageCallback)
    {
        subscribersRepository.Remove(commandType, messageCallback);
    }
}