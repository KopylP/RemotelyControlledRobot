using System.Net;
using System.Threading.Channels;
using Iot.Device.Media;
using Microsoft.AspNetCore.SignalR.Client;
using RemotelyControlledRobot.Camera.Framework;

Console.WriteLine("Start hub connection.");
var connection = new HubConnectionBuilder()
    .WithUrl("http://192.168.0.31:5047/camera")
    .WithAutomaticReconnect()
    .Build();
Console.WriteLine("Connection started.");

await connection.StartAsync();

using var camera = new Camera();
camera.StartCapture();

var channel = Channel.CreateUnbounded<byte[]>();

try
{
    await connection.SendAsync("UploadStream", channel.Reader);
    var frameNumber = 0;

    camera.NewImageReady += (object sender, NewImageBufferReadyEventArgs e) =>
    {
        frameNumber = frameNumber % 2;

        if (frameNumber == 0)
        {
            Console.WriteLine("Send " + e.ImageBuffer.Length + "bytes.");
            channel.Writer.WriteAsync(e.ImageBuffer)
                .AsTask().Wait();
        }

        frameNumber++;
    };

    while (true) { }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    Console.WriteLine("Finally...");
    camera.StopCapture();
    channel.Writer.Complete();
    await connection.DisposeAsync();
}

