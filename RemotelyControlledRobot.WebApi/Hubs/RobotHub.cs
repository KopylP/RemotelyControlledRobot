using Microsoft.AspNetCore.SignalR;

namespace RemotelyControlledRobot.WebApi.Hubs
{
	public class RobotHub : Hub
	{
		public RobotHub()
		{
		}

        public async Task SendSpeedAndDirection(double speed, double direction)
			=> await Clients.All.SendAsync("ReceiveSpeedAndDirection", speed, direction);

        public async Task SendCameraAngle(int angleX, int angleY)
			=> await Clients.All.SendAsync("ReceiveCameraAngle", angleX, angleY);
    }
}