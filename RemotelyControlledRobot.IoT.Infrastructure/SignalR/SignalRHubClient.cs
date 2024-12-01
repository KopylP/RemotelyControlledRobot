using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.IoT.Application.SignalR;

namespace RemotelyControlledRobot.IoT.Infrastructure.SignalR;

internal class SignalRHubClient(HubConnection connection) : ISignalRClient
{
    public async Task ConnectAsync()
    {
        await connection.StartAsync();
    }

    public async Task SendCommandAsync<T>(string methodName, T payload)
    {
        await connection.InvokeAsync(methodName, payload);
    }

    public Task SubscribeToEventAsync<T>(string eventName, Func<T, Task> handler)
    {
        connection.On(eventName, handler);
        return Task.CompletedTask;    
    }
    
    public Task SubscribeToEventAsync<TFirst, TSecond>(string eventName, Func<TFirst, TSecond, Task> handler)
    {
        connection.On(eventName, handler);
        return Task.CompletedTask;    
    }
}