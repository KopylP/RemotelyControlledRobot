namespace RemotelyControlledRobot.IoT.Hardware.Settings;

public record CameraNeckSettings
{
    public static string Section => "CameraNeck";

    public int LeftRightServoPin { get; set; }
    public int UpDownServoPin { get; set; }
    public int LeftRightServoDefaultAngle { get; set; }
    public int UpDownServoDefaultAngle { get; set; }
    public bool ReverseLeftRightServoAngle { get; set; }
    public bool ReverseUpDownServoAngle { get; set; }
}