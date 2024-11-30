namespace RemotelyControlledRobot.Framework.System.Abstractions.Bus;

public interface ICommandPublisher
{
    Task PublishAsync(string command, object? message = null);
}