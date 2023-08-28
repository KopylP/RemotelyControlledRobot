using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework;
using RemotelyControlledRobot.IoT.Core;

ColoredConsole.WriteLineYellow("Initializing robot...");
var application = new RobotApplicationBuilder(new ServiceCollection())
    .AddSignalR()
    .RegisterServices()
    .Build();

await application.RunAsync();

Environment.Exit(0);