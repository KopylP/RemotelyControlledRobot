using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using RemotelyControlledRobot.WebApi.Services;

namespace RemotelyControlledRobot.WebApi.Hubs
{
    public class CameraHub : Hub
    {
        private readonly StreamManager _streamMamager;

		public CameraHub(StreamManager streamMamager)
		{
            _streamMamager = streamMamager;
		}

        public async Task ReadyToStream(string streamName)
        {
            _streamMamager.StartStream(streamName, Context.ConnectionId);
            await Task.CompletedTask;
        }

        public async Task RequestJoinToStream(string streamName)
        {
            _streamMamager.Wait(streamName, Context.ConnectionId);
            var streamerConnectionId = _streamMamager.GetStreamerByStreamName(streamName);

            await Clients
                .Client(streamerConnectionId)
                .SendAsync("PrepareStream");
        }

        public ChannelReader<string> DownloadStream(CancellationToken cancellationToken)
        {
            var channel = _streamMamager.Subscribe(Context.ConnectionId, cancellationToken);
            return channel;
        }

        public async Task UploadStream(ChannelReader<string> channel, string streamName)
		{
            var streamTask = _streamMamager.RunStreamAsync(streamName, channel.ReadAllAsync());
            await Clients.All.SendAsync("StreamReady");

            try
            {
                await streamTask;
            }
            catch (OperationCanceledException)
            {
                _streamMamager.FinishStream(streamName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var streamer = _streamMamager.GetStreamerByParticipant(Context.ConnectionId);

            if (streamer != string.Empty && streamer != Context.ConnectionId)
                await Clients.Client(streamer).SendAsync("SubscriberDisconnected");
        }
    }
}

