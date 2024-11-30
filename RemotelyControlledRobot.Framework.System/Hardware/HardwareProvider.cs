using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

namespace RemotelyControlledRobot.Framework.System.Hardware;

internal sealed class HardwareProvider(IEnumerable<IHardware> hardwares) : IHardwareProvider
{
	public T GetRequiredHardware<T>() where T : IHardware
	{
		return (T) hardwares.First(p => p is T);
	}
}