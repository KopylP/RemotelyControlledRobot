using System;
namespace RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;

public interface IController
{
	public Task HandleAsync(CancellationToken cancellationToken);
}


