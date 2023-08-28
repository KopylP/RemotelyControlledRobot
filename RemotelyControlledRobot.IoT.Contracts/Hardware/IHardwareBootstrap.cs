namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
	public interface IHardwareBootstrap
	{
		public void Initialize();
		public void Stop();
	}
}