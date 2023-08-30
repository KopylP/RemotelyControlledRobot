using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.IoT.Core;

var application = new RobotApplicationBuilder(new ServiceCollection())
    .AddCommandBus()
    .AddSignalR()
    .RegisterServices()
    .Build();

await application.RunAsync();

Environment.Exit(0);