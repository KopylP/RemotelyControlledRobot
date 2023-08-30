using System;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace RemotelyControlledRobot.WebApi.Hubs
{
	public class CameraHub : Hub
    {
        private static Channel<string>? _clientChannel;

		public CameraHub(ILogger<CameraHub> logger)
		{
		}

        public ChannelReader<string> DownloadStream(CancellationToken cancellationToken)
        {
            _clientChannel = Channel.CreateUnbounded<string>();
            return _clientChannel.Reader;
        }

        public async Task UploadStream(ChannelReader<byte[]> channel)
		{
            while (await channel.WaitToReadAsync())
            {
                // Read all currently available data synchronously, before waiting for more data
                while (channel.TryRead(out var frame))
                {
                    if (_clientChannel != null)
                    {
                        await _clientChannel.Writer.WriteAsync($"data:image/png;base64,{Convert.ToBase64String(frame)}");
                    }
                }
            }
        }
    }
}

