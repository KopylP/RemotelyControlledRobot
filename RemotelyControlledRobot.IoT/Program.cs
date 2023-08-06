using Microsoft.Extensions.DependencyInjection;
using RemotelyControlledRobot.IoT.Core;

var application = new RobotApplicationBuilder(new ServiceCollection())
    .RegisterGpioController()
    .RegisterServices()
    .Build();

await Task.WhenAll(application.RunAsync());
Console.Write("To exit press any key");
Console.ReadKey();
