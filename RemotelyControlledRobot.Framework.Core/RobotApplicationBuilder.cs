using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.Framework.Application.Controllers;
using RemotelyControlledRobot.Framework.System;

namespace RemotelyControlledRobot.Framework.Core;

public class RobotApplicationBuilder
{
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }

    private RobotApplicationBuilder(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }

    public static RobotApplicationBuilder CreateMinimal(IConfigurationBuilder configurationBuilder, params Type[] assemblyReferences)
    {
        var services = new ServiceCollection();
        var configuration = configurationBuilder.Build();
        
        var builder = new RobotApplicationBuilder(services, configuration);

        builder.Services.AddScoped<IConfiguration>(_ => configuration);
        builder.Services.AddSystem(assemblyReferences);
        builder.Services.AddControllers(configuration, assemblyReferences);

        return builder;
    }

    public IRobotApplication Build()
    {
        var serviceProvider = Services.BuildServiceProvider();
        return ActivatorUtilities.CreateInstance<RobotApplication>(serviceProvider);
    }
}