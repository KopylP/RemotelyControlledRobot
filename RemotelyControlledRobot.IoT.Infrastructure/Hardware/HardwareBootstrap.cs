using System.Device.Gpio;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Contracts.Hardware;

namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware
{
    public sealed class HardwareBootstrap : IHardwareBootstrap
    {
        private readonly IEnumerable<IHardware> _hardwareComponents;
        private readonly GpioController _gpioController;

        public HardwareBootstrap(IEnumerable<IHardware> hardwareComponents, GpioController gpioController)
        {
            _hardwareComponents = hardwareComponents;
            _gpioController = gpioController;
        }

        public void Initialize()
        {
            foreach (var hardwareComponent in _hardwareComponents)
            {
                if (hardwareComponent is not HardwareBase)
                {
                    throw new ArgumentException($"Hardware component should implement {typeof(HardwareBase)}");
                }

                (hardwareComponent as HardwareBase)!.Initialize(_gpioController);
                ColoredConsole.WriteLineGreen($"{hardwareComponent.GetType().Name} hardware was initialized.");
            }
        }

        public void Stop()
        {
            foreach (var hardwareComponent in _hardwareComponents)
            {
                (hardwareComponent as HardwareBase)!.OnStop(_gpioController);
                ColoredConsole.WriteLineRed($"{hardwareComponent.GetType().Name} hardware was stopped.");
            }
        }
    }
}
