namespace RemotelyControlledRobot.IoT.Core;

public static class AssemblyMarkers
{
    private static Type Application => typeof(Application.AssemblyMarker);
    private static Type Infrastructure => typeof(Infrastructure.AssemblyMarker);
    private static Type Hardware => typeof(Hardware.AssemblyMarker);

    public static readonly Type[] All = [Application, Infrastructure, Hardware];
}