using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.Framework.Application;
using RemotelyControlledRobot.IoT.Application;
using RemotelyControlledRobot.IoT.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.SignalR;

using ApplicationAssemblyMarker = RemotelyControlledRobot.IoT.Application.AssemblyMarker;
using InfrastructureAssemblyMarker = RemotelyControlledRobot.IoT.Infrastructure.AssemblyMarker;
using HardwareAssemblyMarker = RemotelyControlledRobot.IoT.Hardware.AssemblyMarker;

static IConfigurationBuilder BuildConfiguration() => new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false);

static bool IsSignalREnabled(IConfiguration configuration) => 
    configuration.GetValue<bool>(ConfigurationKeys.SignalREnabled);

Type[] assemblyMarkers = [
    ApplicationAssemblyMarker.Type,
    InfrastructureAssemblyMarker.Type,
    HardwareAssemblyMarker.Type,
];

var builder = RobotApplicationBuilder
    .CreateMinimal(BuildConfiguration(), assemblyMarkers);

if (IsSignalREnabled(builder.Configuration))
{
    builder.Services.AddSignalR(builder.Configuration[ConfigurationKeys.SignalRHost]!);
}

builder.Services.AddHardware(builder.Configuration);

var app = builder.Build();

await app.RunAsync();

Environment.Exit(0);