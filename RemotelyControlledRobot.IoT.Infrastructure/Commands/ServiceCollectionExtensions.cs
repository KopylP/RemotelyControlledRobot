using System;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.IoT.Contracts.Commands;

namespace RemotelyControlledRobot.IoT.Infrastructure.Commands
{
	public static class ServiceCollectionExtensions
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
            services.AddSingleton<ICommandSubscribersRepository, CommandSubscribersRepository>();
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<ICommandPublisher, CommandPublisher>();
            services.AddTransient<ICommandSubscriber, CommandsSubscriber>();

            return services;
        }
    }
}
