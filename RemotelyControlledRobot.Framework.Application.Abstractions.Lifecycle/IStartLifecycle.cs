namespace RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

public interface IStartLifecycle
{
    Task OnStartAsync(CancellationToken cancellationToken);
}