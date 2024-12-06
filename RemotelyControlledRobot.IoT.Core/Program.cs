using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.Framework.Core;
using RemotelyControlledRobot.IoT.Application;
using RemotelyControlledRobot.IoT.Core;
using RemotelyControlledRobot.IoT.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.SignalR;

static IConfigurationBuilder BuildConfiguration() => new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile(path: "appsettings.json", optional: false);

var builder = RobotApplicationBuilder
    .CreateMinimal(BuildConfiguration(), AssemblyMarkers.All);

if (IsSignalREnabled(builder.Configuration))
{
    builder.Services.AddSignalR(builder.Configuration[ConfigurationKeys.SignalRHost]!);
}

builder.Services.AddHardware(builder.Configuration);

var app = builder.Build();

await app.RunAsync();

Environment.Exit(0);
return;

static bool IsSignalREnabled(IConfiguration configuration) => 
    configuration.GetValue<bool>(ConfigurationKeys.SignalREnabled);