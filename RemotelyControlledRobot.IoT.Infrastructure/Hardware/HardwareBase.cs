using System.Device.Gpio;

namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
	public class HardwareBase
	{
		public virtual void Initialize(GpioController gpioController) { }
		public virtual void OnStop(GpioController gpioController) { }
	}
}