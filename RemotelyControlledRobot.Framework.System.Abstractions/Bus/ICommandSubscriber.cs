namespace RemotelyControlledRobot.Framework.System.Abstractions.Bus;

public interface ICommandSubscriber
{
    void Subscribe(string commandType, Action<object?> messageCallback);
    void Unsubscribe(string commandType, Action<object?> messageCallback);
}