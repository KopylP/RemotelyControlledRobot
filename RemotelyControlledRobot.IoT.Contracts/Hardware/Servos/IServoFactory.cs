using Iot.Device.ServoMotor;

namespace RemotelyControlledRobot.IoT.Contracts.Hardware.Servos
{
    public interface IServoFactory
    {
        public ServoMotor CreateSoftwareSW90Servo(int pinNumber);
    }
}