using System.Net;
using System.Threading.Channels;
using Iot.Device.Media;
using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.IoT.Camera.SignalRStream;

const string streamName = "ROBOT_CAMERA";

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

await connection.SendAsync("ReadyToStream", streamName);

Channel<string>? channel = null;
connection.On("PrepareStream", async () =>
{
    camera.StartCapture();
    channel = Channel.CreateBounded<string>(new BoundedChannelOptions(capacity: 10)
    {
        FullMode = BoundedChannelFullMode.DropOldest,
    });

    camera.NewImageReady += CameraOnNewImageReady;

    await connection.SendAsync("UploadStream", channel.Reader, streamName);
});

connection.On("SubscriberDisconnected", async () =>
{
    CloseStream();
    await ReadyToStreamAsync();
});

connection.Reconnecting += ConnectionReconnecting;

connection.Reconnected += ConnectionReconnected;

;

while (true) { }

async Task ConnectionReconnecting(Exception? arg)
{
    CloseStream();
    await Task.CompletedTask;
}

async Task ConnectionReconnected(string? arg)
{
    await ReadyToStreamAsync();
}

void CameraOnNewImageReady(object sender, NewImageBufferReadyEventArgs e)
{
    try
    {
        Console.WriteLine("Send " + e.ImageBuffer.Length + "bytes.");
        channel?.Writer.TryWrite("data:image/png;base64," + Convert.ToBase64String(e.ImageBuffer));
    }
    catch (ChannelClosedException)
    {
        CloseStream();
        ReadyToStreamAsync().GetAwaiter().GetResult();
    }
}

void CloseStream()
{
    Console.WriteLine("Stream closed.");
    camera!.StopCapture();
    camera.NewImageReady -= CameraOnNewImageReady;
    channel?.Writer.TryComplete();

    channel = null;
}

async Task ReadyToStreamAsync()
{
    await connection!.SendAsync("ReadyToStream", streamName);
}

