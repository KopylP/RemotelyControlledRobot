using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

namespace RemotelyControlledRobot.Framework.Core;

internal class RobotApplication(
    IEnumerable<IController> controllers,
    IEnumerable<IStartLifecycle> startLifecycles,
    IEnumerable<IStopLifecycle> stopLifecycles) 
    : IRobotApplication
{
    private readonly CancellationTokenSource _cts = new();
    
    public async Task RunAsync()
    {
        try 
        {
            await StartApplicationAsync();
            await RunAndWaitForControllerHandlersAsync();
        }
        finally 
        {
            await StopApplicationAsync();
        }
    }

    private async Task StartApplicationAsync()
    {
        foreach (var startLifecycle in startLifecycles)
        {
            await startLifecycle.OnStartAsync(_cts.Token);
        }
    }

    private async Task StopApplicationAsync()
    {
        await _cts.CancelAsync();
        
        foreach (var stopLifecycle in stopLifecycles)
        {
            await stopLifecycle.OnStopAsync();
        }
        
        ColoredConsole.WriteLineRed("Robot application has been stopped.");
    }
    private async Task RunAndWaitForControllerHandlersAsync()
    {
        ColoredConsole.WriteLineYellow("Running controller handlers...");
        var tasks = controllers
            .Select(controller => controller
                .HandleAsync(_cts.Token));
        ColoredConsole.WriteLineGreen("Robot application started successfully.");

        await Task.WhenAll(tasks);
    }
}