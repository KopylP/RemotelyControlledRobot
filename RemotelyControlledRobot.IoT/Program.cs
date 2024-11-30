using Microsoft.Extensions.Configuration;
using RemotelyControlledRobot.Framework.Application;
using RemotelyControlledRobot.Framework.Application.Lifecycle;
using RemotelyControlledRobot.IoT.Application;
using RemotelyControlledRobot.IoT.Application.SignalR.Lifecycle;
using RemotelyControlledRobot.IoT.Infrastructure.Hardware;
using RemotelyControlledRobot.IoT.Infrastructure.SignalR;

using ApplicationAssemblyMarker = RemotelyControlledRobot.IoT.Application.AssemblyMarker;
using InfrastructureAssemblyMarker = RemotelyControlledRobot.IoT.Infrastructure.AssemblyMarker;

IConfigurationBuilder BuildConfiguration() => new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false);

Type[] assemblyMarkers = [
    ApplicationAssemblyMarker.Type,
    InfrastructureAssemblyMarker.Type,
];
var builder = RobotApplicationBuilder
    .CreateMinimal(BuildConfiguration(), assemblyMarkers);

builder.Services.AddSignalR(builder.Configuration[ConfigurationKeys.SignalRHost]!);
builder.Services.AddStartLifecycle<SignalRBootstrapper>();
builder.Services.AddHardware(builder.Configuration);

var app = builder.Build();

await app.RunAsync();

Environment.Exit(0);