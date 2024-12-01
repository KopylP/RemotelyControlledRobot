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
        RegisterCancelKeyPressHandler();

        try 
        {
            await StartApplicationAsync();
            await RunAndWaitForControllerHandlersAsync();
        }
        catch (Exception ex)
        {
            ColoredConsole.WriteLineRed($"Unhandled exception: {ex.Message}");
            throw;
        }
        finally 
        {
            await StopApplicationAsync();
            UnregisterCancelKeyPressHandler();
        }
    }

    private void RegisterCancelKeyPressHandler()
    {
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    private void UnregisterCancelKeyPressHandler()
    {
        Console.CancelKeyPress -= OnCancelKeyPress;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        ColoredConsole.WriteLineYellow("Application shutdown requested.");
        
        e.Cancel = true;
        _cts.Cancel();
        
        UnregisterCancelKeyPressHandler();
        StopApplicationAsync().GetAwaiter().GetResult();
        
        Environment.Exit(0);
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
        
        ColoredConsole.WriteLineRed("Robot application was stopped.");
    }

    private async Task RunAndWaitForControllerHandlersAsync()
    {
        ColoredConsole.WriteLineYellow("Running controller handlers...");
        
        var tasks = controllers
            .Select(controller => controller.HandleAsync(_cts.Token));
        
        ColoredConsole.WriteLineGreen("Robot application started successfully.");

        await Task.WhenAll(tasks);
    }
}