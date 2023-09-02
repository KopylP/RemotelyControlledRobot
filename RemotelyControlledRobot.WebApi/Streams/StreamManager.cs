using System.Threading.Channels;

namespace RemotelyControlledRobot.WebApi.Services
{
    public class StreamManager
	{
        private readonly List<Streams.Stream> _streams = new();

        public StreamManager()
		{
		}

		public void StartStream(string streamName, string connectionId)
		{
			RemoveOldStream(streamName);
			AddNewStream(streamName, connectionId);
		}

        public string GetStreamerByStreamName(string streamName)
		{
            var stream = _streams.First(p => p.Name == streamName);
			return stream.Streamer;
        }

		public string GetStreamerByParticipant(string participantId)
		{
			var stream = _streams.FirstOrDefault(p => p.Participants.Contains(participantId));
			return stream?.Streamer ?? string.Empty;
		}

		public void Wait(string streamName, string waitingId)
		{
            var stream = _streams.First(p => p.Name == streamName);
			stream.Wait(waitingId);
        }

        public async Task RunStreamAsync(string streamName, IAsyncEnumerable<string> streamChannel)
		{
            var stream = _streams.First(p => p.Name == streamName);
			await stream.RunStreamAsync(streamChannel);
		}

		public ChannelReader<string> Subscribe(string subscriberId, CancellationToken cancellationToken)
		{
            var stream = _streams.First(p => p.Waiting == subscriberId);
			var reader = stream.Subscribe(subscriberId);

            cancellationToken.Register(stream.Stop);

			return reader; 
        }

        public IEnumerable<string> FinishStream(string streamName)
        {
            var stream = _streams.FirstOrDefault(p => p.Name == streamName);

			if (stream is not null)
			{
				stream.Stop();
				_streams.Remove(stream);
			}

			return stream?.Participants ?? Array.Empty<string>();
        }

		private void RemoveOldStream(string streamName)
		{
            var oldStream = _streams.FirstOrDefault(p => p.Name == streamName);

			if (oldStream != null)
				_streams.Remove(oldStream);
        }

        private void AddNewStream(string streamName, string connectionId)
        {
            var stream = new Streams.Stream(streamName, connectionId);
            _streams.Add(stream);
        }
    }
}


