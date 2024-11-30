using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Main.Controllers
{
	[ControllerEnable(ConfigurationKeys.LoopControllerEnabled)]
	public class LoopController : ControllerBase, IController
    {
        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            while(!cancellationToken.IsCancellationRequested)
            {
                // Loop
            }
        }
    }
}

