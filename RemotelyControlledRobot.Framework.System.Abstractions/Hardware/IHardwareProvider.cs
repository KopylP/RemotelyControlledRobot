namespace RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

public interface IHardwareProvider
{
    T GetRequiredHardware<T>() where T : IHardware;
}