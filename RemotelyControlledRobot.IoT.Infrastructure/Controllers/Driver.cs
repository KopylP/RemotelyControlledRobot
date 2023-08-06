using System.Device.Gpio;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Infrastructure.Controllers
{
	public class Driver : IDriver
	{
		private readonly GpioController _gpioController;

		public Driver(GpioController gpioController)
		{
			_gpioController = gpioController;
		}
	}
}

