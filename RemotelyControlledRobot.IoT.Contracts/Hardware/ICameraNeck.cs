namespace RemotelyControlledRobot.IoT.Contracts.Hardware
{
	public interface ICameraNeck : IHardware
    {
        void TurnLeftMax();
        void TurnAhead();
        void TurnRightMax();
        void WriteXAngle(int angle);
        void WriteYAngle(int angle);
    }
}
