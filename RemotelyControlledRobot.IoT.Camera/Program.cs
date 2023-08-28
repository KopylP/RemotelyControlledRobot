using System.Net;
using System.Text;
using Iot.Device.Media;
using RemotelyControlledRobot.Camera.Framework;
using static Iot.Device.Media.VideoDevice;


using var camera = new Camera();
camera.StartCapture();

Console.WriteLine("Listening for video stream...");
HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://*:8080/");
listener.Start();

try
{
    while (true)
    {
        HttpListenerContext context = await listener.GetContextAsync();
        Console.WriteLine($"Client connected from {context.Request.RemoteEndPoint}");

        _ = Task.Run(() => ProcessRequest(context, camera));
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex}");
}
finally
{
    camera.StopCapture();
    listener.Stop();
}

static void ProcessRequest(HttpListenerContext context, Camera camera)
{
    CancellationTokenSource cancellationTokenSource = new();
    var token = cancellationTokenSource.Token;

    Console.WriteLine($"Streaming to client {context.Request.RemoteEndPoint}");

    context.Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";
    context.Response.Headers.Add("Connection", "Keep-Alive");
    context.Response.Headers.Add("Cache-Control", "no-cache");

    NewImageBufferReadyEvent @event = (sender, e) =>
    {
        try
        {
            WriteFrame(context.Response.OutputStream, e);
        }
        catch (HttpListenerException)
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine($"Client {context.Request.RemoteEndPoint} disconnected.");
                cancellationTokenSource.Cancel();
            }
        }
    };

    camera.NewImageReady += @event;

    while (!token.IsCancellationRequested)
    {
    }

    camera.NewImageReady -= @event;

    context.Response.Close();
}

static void WriteFrame(Stream outputStream, NewImageBufferReadyEventArgs e)
{
    WriteHeader(outputStream, e.Length);
    outputStream.Write(e.ImageBuffer, 0, e.Length);
    WriteFooter(outputStream);
    outputStream.Flush();
}

static void WriteHeader(Stream outputStream, int length)
{
    string header = $"--frame\r\nContent-Type: image/jpeg\r\nContent-Length: {length}\r\n\r\n";
    byte[] headerBytes = Encoding.ASCII.GetBytes(header);
    outputStream.Write(headerBytes, 0, headerBytes.Length);
}

static void WriteFooter(Stream outputStream)
{
    byte[] footerBytes = Encoding.ASCII.GetBytes("\r\n");
    outputStream.Write(footerBytes, 0, footerBytes.Length);
}
