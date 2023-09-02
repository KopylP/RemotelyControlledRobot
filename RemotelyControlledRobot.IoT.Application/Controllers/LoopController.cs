using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Application.Controllers
{
	[ControllerConfiguration("Loop")]
	public class LoopController : ControllerBase, IController
    {
		public LoopController()
		{
		}

        public async override Task HandleAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            while(!cancellationToken.IsCancellationRequested)
            {
                // Loop
            }
        }
    }
}

