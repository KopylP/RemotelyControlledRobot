using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware
{
	public sealed class HardwareProvider : IHardwareProvider
	{
        private readonly IEnumerable<IHardware> _hardwares;

		public HardwareProvider(IEnumerable<IHardware> hardwares)
		{
            _hardwares = hardwares;
		}

        public T GetRequiredHardware<T>() where T : IHardware
        {
            return (T) _hardwares.First(p => typeof(T).IsAssignableFrom(p.GetType()));
        }
    }
}