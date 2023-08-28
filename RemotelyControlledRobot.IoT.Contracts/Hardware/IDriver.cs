namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
    public interface IDriver : IHardware
    {
        public void GoAhead();
        public void GoLeft();
        public void GoRight();
        public void Stop();
        public void GoBack();
        void SetSpeedWithDirection(double speed, double direction);
    }
}