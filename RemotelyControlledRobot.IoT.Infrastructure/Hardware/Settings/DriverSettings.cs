namespace RemotelyControlledRobot.IoT.Infrastructure.Hardware.Settings
{
    public record DriverSettings
    {
        public static string Section => "Driver";

        public int IN1 { get; set; }
        public int IN2 { get; set; }
        public int ENA { get; set; }

        public int IN4 { get; set; }
        public int IN3 { get; set; }
        public int ENB { get; set; }

        public double CalibrationCoefficient { get; set; }
    }
}

