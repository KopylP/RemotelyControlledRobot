namespace RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

public interface IStopLifecycle
{
    Task OnStopAsync();
}