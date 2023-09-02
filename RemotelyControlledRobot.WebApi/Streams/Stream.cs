using System;
using System.Threading.Channels;

namespace RemotelyControlledRobot.WebApi.Streams
{
	public class Stream
	{
		private readonly ISet<Participant> _partisipants = new HashSet<Participant>();
        private ChannelWriter<string>? _clientChannel = null;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public string Name { get; }
		public IEnumerable<string> Participants => _partisipants.Select(p => p.Id).ToArray();
		public string Streamer => _partisipants.Where(p => p.IsStreamer).First().Id;
		public string Waiting => _partisipants.Where(p => p.IsWaiting).First().Id;


        public Stream(string streamName, string streamerId)
		{
            Name = streamName;
			_partisipants.Add(Participant.CreateStreamer(streamerId));
		}

		public void Wait(string waitingId)
		{
			_partisipants.Add(Participant.CreateWaiting(waitingId));
		}

		public ChannelReader<string> Subscribe(string clientConnectionId)
		{
			var waiting = _partisipants.FirstOrDefault(p => p.IsWaiting && p.Id == clientConnectionId);

			if (waiting is not null)
				_partisipants.Remove(waiting);

			_partisipants.Add(Participant.CreateSubscriber(clientConnectionId));

			var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(capacity: 10)
			{
				FullMode = BoundedChannelFullMode.DropOldest
			});

			_clientChannel = channel.Writer;
			return channel.Reader;
		}

		public void Stop()
		{
			if (!_cancellationTokenSource.IsCancellationRequested)
			{
				_clientChannel?.TryComplete();
				_cancellationTokenSource.Cancel();
			}
		}

		public async Task RunStreamAsync(IAsyncEnumerable<string> stream)
		{
			await foreach (var item in stream)
			{
				if (_cancellationTokenSource.IsCancellationRequested)
					break;

				_clientChannel?.WriteAsync(item);
			}
		}
	}
}

