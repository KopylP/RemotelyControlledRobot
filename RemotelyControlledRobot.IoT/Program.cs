using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.IoT.Core;

var application = new RobotApplicationBuilder(new ServiceCollection(), CreateConfiguration())
    .RegisterConfiguration()
    .AddCommandBus()
    .AddSignalR()
    .RegisterServices()
    .Build();

await application.RunAsync();

Environment.Exit(0);

IConfiguration CreateConfiguration() => new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();