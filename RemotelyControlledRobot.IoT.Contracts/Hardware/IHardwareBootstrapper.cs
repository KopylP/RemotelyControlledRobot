namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
	public interface IHardwareBootstrapper
	{
		public void Initialize();
		public void Stop();
	}
}