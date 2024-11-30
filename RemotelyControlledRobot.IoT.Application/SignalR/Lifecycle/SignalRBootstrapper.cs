using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.Framework.Application.Abstractions.Lifecycle;

namespace RemotelyControlledRobot.IoT.Application.SignalR.Lifecycle;

public class SignalRBootstrapper(
    IConfiguration configuration, 
    HubConnection hubConnection) : IStartLifecycle
{
    public async Task OnStartAsync(CancellationToken cancellationToken)
    {
        if (IsSignalREnabled())
        {
            ColoredConsole.WriteLineYellow("Starting SignalR connection...");
            await hubConnection.StartAsync(cancellationToken);
            ColoredConsole.WriteLineGreen("SignalR connection started successfully!");
        }
    }
    
    private bool IsSignalREnabled()
    => !configuration.GetValue<bool>(ConfigurationKeys.SignalRIsDisabled);
}