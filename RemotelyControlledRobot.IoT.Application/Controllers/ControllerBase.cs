using System;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
    public class ControllerBase : IController
    {
        public ControllerBase()
        {
        }

        public virtual Task HandleAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

