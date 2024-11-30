using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal class CommandBusBootstrapper(CommandBus commandBus) : IStartLifecycle
{
    public Task OnStartAsync(CancellationToken cancellationToken)
    {
        ColoredConsole.WriteLineYellow("Starting Command Bus...");
        _ = commandBus.ProcessCommandsAsync(cancellationToken);
        ColoredConsole.WriteLineGreen("Command Bus was started");

        return Task.CompletedTask;
    }
}