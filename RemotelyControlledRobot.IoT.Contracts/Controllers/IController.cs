using System;
namespace RemotelyControlledRobot.IoT.Contracts.Controllers
{
	public interface IController
	{
		public Task HandleAsync(CancellationToken cancellationToken);
	}
}

