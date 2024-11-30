using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;

namespace RemotelyControlledRobot.IoT.Infrastructure.SignalR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalR(this IServiceCollection services, string host)
    {
        ColoredConsole.WriteLineYellow("Registering SignalR...");

        var hubConnection = new HubConnectionBuilder()
            .WithUrl(host)
            .WithAutomaticReconnect()
            .Build();

        services.AddSingleton(hubConnection);

        return services;
    }
}