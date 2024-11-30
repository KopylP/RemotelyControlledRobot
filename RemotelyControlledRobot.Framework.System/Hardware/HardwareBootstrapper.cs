using System.Device.Gpio;
using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

namespace RemotelyControlledRobot.Framework.System.Hardware;

internal sealed class HardwareBootstrapper
    (IEnumerable<IHardware> hardwareComponents, GpioController gpioController) : IStopLifecycle, IStartLifecycle
{
    public Task OnStartAsync(CancellationToken cancellationToken)
    {
        foreach (var hardwareComponent in hardwareComponents)
        {
            hardwareComponent.Initialize(gpioController);
            ColoredConsole.WriteLineGreen($"{hardwareComponent.GetType().Name} hardware was initialized.");
        }
        
        return Task.CompletedTask;
    }
    
    public Task OnStopAsync()
    {
        foreach (var hardwareComponent in hardwareComponents)
        {
            hardwareComponent.Stop(gpioController);
            ColoredConsole.WriteLineRed($"{hardwareComponent.GetType().Name} hardware was stopped.");
        }
        
        return Task.CompletedTask;
    }
}