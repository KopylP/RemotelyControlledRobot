using RemotelyControlledRobot.IoT.Abstract;
using RemotelyControlledRobot.IoT.Contracts.Commands;
using RemotelyControlledRobot.IoT.Contracts.Controllers;

namespace RemotelyControlledRobot.IoT.Infrastructure.Controllers
{
	public class MoveController : IMoveController
    {
        private readonly IDriver _driver;
        private readonly ICommandBus _commandBus;

        public MoveController(IDriver driver, ICommandBus commandBus)
		{
            _commandBus = commandBus;
            _driver = driver;
		}

        public void Initialize()
        {
            _commandBus.Subscribe(MoveControllerCommands.Ahead, OnAhead);
            _commandBus.Subscribe(MoveControllerCommands.Stop, OnStop);
            _commandBus.Subscribe(MoveControllerCommands.Left, OnLeft);
            _commandBus.Subscribe(MoveControllerCommands.Right, OnRight);
        }

        private void OnRight(string arg)
        {
            Console.WriteLine("Go right");
        }

        private void OnLeft(string arg)
        {
            Console.WriteLine("Go left");
        }

        private void OnStop(string arg)
        {
            Console.WriteLine("Stop");
        }

        private void OnAhead(string arg)
        {
            Console.WriteLine("Go ahead");
        }
    }
}

