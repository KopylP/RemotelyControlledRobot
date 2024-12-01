namespace RemotelyControlledRobot.IoT.Application.SignalR;

public interface ISignalRClient
{
    Task ConnectAsync();
    Task SendCommandAsync<T>(string methodName, T payload);
    Task SubscribeToEventAsync<T>(string eventName,  Func<T, Task> handler);
    Task SubscribeToEventAsync<TFirst, TSecond>(string eventName, Func<TFirst, TSecond, Task> handler);
}