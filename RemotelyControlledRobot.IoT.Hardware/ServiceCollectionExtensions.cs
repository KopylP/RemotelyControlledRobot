using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.IoT.Hardware.Cameras;
using RemotelyControlledRobot.IoT.Hardware.Settings;

namespace RemotelyControlledRobot.IoT.Hardware;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHardware(this IServiceCollection services, IConfiguration configuration)
    {
        var cameraNeckSettings = new CameraNeckSettings();
        var driverSettings = new DriverSettings();
        var videoCameraSettings = new VideoCameraSettings();

        configuration.GetSection(CameraNeckSettings.Section)
            .Bind(cameraNeckSettings);
        configuration.GetSection(DriverSettings.Section)
            .Bind(driverSettings);
        configuration.GetSection(VideoCameraSettings.Section)
            .Bind(videoCameraSettings);

        services.AddSingleton(cameraNeckSettings);
        services.AddSingleton(driverSettings);
        services.AddSingleton(videoCameraSettings);

        return services;
    }
}