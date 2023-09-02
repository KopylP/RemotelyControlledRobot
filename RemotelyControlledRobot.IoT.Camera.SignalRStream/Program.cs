using System.Net;
using System.Threading.Channels;
using Iot.Device.Media;
using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Camera.Framework;

const string STREAM_NAME = "ROBOT_CAMERA";

Console.WriteLine("Start hub connection.");
var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost/camera")
    .WithAutomaticReconnect()
    .Build();
Console.WriteLine("Connection started.");

await connection.StartAsync();

using var camera = new Camera();
Console.WriteLine("Starting camera device...");
Console.WriteLine("Waiting 10 seconds before camera is calibrated...");
Thread.Sleep(10_000);
Console.WriteLine("Camera is ready");

await connection.SendAsync("ReadyToStream", STREAM_NAME);

Channel<string>? channel = null;
connection.On("PrepareStream", async () =>
{
    camera.StartCapture();
    channel = Channel.CreateBounded<string>(new BoundedChannelOptions(capacity: 10)
    {
        FullMode = BoundedChannelFullMode.DropOldest,
    });

    camera.NewImageReady += (object sender, NewImageBufferReadyEventArgs e) =>
    {
        try
        {
            Console.WriteLine("Send " + e.ImageBuffer.Length + "bytes.");
            channel.Writer.TryWrite("data:image/png;base64," + Convert.ToBase64String(e.ImageBuffer));
        }
        catch (ChannelClosedException)
        {
            CloseStream();
            ReadyToStreamAsync().GetAwaiter().GetResult();
        }
    };

    await connection.SendAsync("UploadStream", channel.Reader, STREAM_NAME);
});

connection.On("SubscriberDisconnected", async () =>
{
    CloseStream();
    await ReadyToStreamAsync();
});

connection.Reconnecting += Connection_Reconnecting;

connection.Reconnected += Connection_Reconnected;

async Task Connection_Reconnected(string? arg)
{
    await ReadyToStreamAsync();
};

async Task Connection_Reconnecting(Exception? arg)
{
    CloseStream();
    await Task.CompletedTask;
}

while (true) { }

void CloseStream()
{
    Console.WriteLine("Stream closed.");
    camera!.StopCapture();
    camera.ClearSubscribers();
    channel?.Writer.TryComplete();

    channel = null;
}

async Task ReadyToStreamAsync()
{
    await connection!.SendAsync("ReadyToStream", STREAM_NAME);
}

