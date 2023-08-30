namespace RemotelyControlledRobot.IoT.Contracts.Commands
{
	public interface ICommandBus
	{
        Task ProcessCommandsAsync(CancellationToken cancellationToken);
    }
}

