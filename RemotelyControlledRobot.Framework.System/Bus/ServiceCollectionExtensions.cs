using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Lifecycle;
using RemotelyControlledRobot.Framework.System.Abstractions.Bus;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal static class ServiceCollectionExtensions
{
    private static BoundedChannelOptions ChannelOptions => new(capacity: 25)
    {
        SingleReader = true,
        SingleWriter = true,
        FullMode = BoundedChannelFullMode.DropOldest
    };

    public static IServiceCollection AddCommandBus(this IServiceCollection services)
    {
        var channel = Channel.CreateBounded<(string Command, object? Message)>(ChannelOptions);

        services.AddSingleton(channel.Reader);
        services.AddSingleton(channel.Writer);
        services.AddSingleton<CommandSubscribersRepository>();
        services.AddTransient<CommandBus>();
        services.AddTransient<ICommandPublisher, CommandPublisher>();
        services.AddTransient<ICommandSubscriber, CommandsSubscriber>();
        services.AddStartLifecycle<CommandBusBootstrapper>();

        return services;
    }
}