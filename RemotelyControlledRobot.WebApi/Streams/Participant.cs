using System;
namespace RemotelyControlledRobot.WebApi.Streams
{

	public record Participant
	{
        private enum ParticipantType
        {
            Streamer,
            Subscriber,
            Waiting,
        };
        private readonly ParticipantType _type;

        public string Id { get; }

        private Participant(string participantId, ParticipantType type)
		{
            Id = participantId;
            _type = type;
		}

        public bool IsStreamer => _type == ParticipantType.Streamer;
        public bool IsWaiting => _type == ParticipantType.Waiting;

        public static Participant CreateStreamer(string participantId) => new Participant(participantId, ParticipantType.Streamer);
        public static Participant CreateSubscriber(string participantId) => new Participant(participantId, ParticipantType.Subscriber);
        public static Participant CreateWaiting(string participantId) => new Participant(participantId, ParticipantType.Waiting);
    }
}

