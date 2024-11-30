using System.Device.Gpio;

namespace RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

public interface IHardware
{
    void Initialize(GpioController controller);
    void Stop(GpioController controller);
}