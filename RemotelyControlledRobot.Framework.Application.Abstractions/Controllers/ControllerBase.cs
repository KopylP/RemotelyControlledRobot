namespace RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;

public class ControllerBase : IController
{
    public virtual Task HandleAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}