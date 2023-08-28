namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
	public interface IHardwareProvider
	{
		public T GetRequiredHardware<T>() where T : IHardware;
	}
}

