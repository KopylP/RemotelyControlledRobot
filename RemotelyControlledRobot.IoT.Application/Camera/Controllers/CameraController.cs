using System.Net;
using System.Text;
using Iot.Device.Media;
using RemotelyControlledRobot.Framework.Application.Abstractions.Controllers;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;
using RemotelyControlledRobot.IoT.Hardware.Cameras;

namespace RemotelyControlledRobot.IoT.Application.Camera.Controllers;

public class CameraController(IHardwareProvider provider) : ControllerBase
{
    private readonly VideoCamera _videoCamera 
        = provider.GetRequiredHardware<VideoCamera>();

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        _ = Task.Run(() => _videoCamera.CaptureContinuous(cancellationToken), cancellationToken);
        await Task.Delay(3000, cancellationToken);
        
        using var listener = new HttpListener();
        listener.Prefixes.Add("http://*:8080/");
        listener.Start();
        Console.WriteLine("HTTP server started on http://*:8080/");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await listener.GetContextAsync();
                _ = ProcessRequestAsync(context, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Server shutdown requested.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Client connected: {context.Request.RemoteEndPoint}");
        context.Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";
        context.Response.Headers["Connection"] = "Keep-Alive";
        context.Response.Headers["Cache-Control"] = "no-cache";
        
        await using var outputStream = context.Response.OutputStream;

        try
        {
            _videoCamera.NewImageBufferReadyEvent += VideoCameraOnNewImageBufferReadyEvent;

            while (!cancellationToken.IsCancellationRequested)
            {
            }

            _videoCamera.NewImageBufferReadyEvent -= VideoCameraOnNewImageBufferReadyEvent;
        }
        catch (Exception ex)
        {
            _videoCamera.NewImageBufferReadyEvent -= VideoCameraOnNewImageBufferReadyEvent;
            Console.WriteLine($"Error streaming to {context.Request.RemoteEndPoint}: {ex.Message}");
        }
        finally
        {
            context.Response.Close();
            Console.WriteLine($"Client disconnected: {context.Request.RemoteEndPoint}");
        }

        return;

        void VideoCameraOnNewImageBufferReadyEvent(object sender, NewImageBufferReadyEventArgs e)
        {
            WriteFrame(outputStream, e, cancellationToken);
        }
    }

    private static void WriteFrame(Stream outputStream, NewImageBufferReadyEventArgs frame, CancellationToken cancellationToken)
    {
        var header = Encoding.ASCII.GetBytes($"--frame\r\nContent-Type: image/jpeg\r\nContent-Length: {frame.Length}\r\n\r\n");
        outputStream.Write(header);
        outputStream.Write(frame.ImageBuffer);
        var footer = Encoding.ASCII.GetBytes("\r\n");
        outputStream.Write(footer);
        outputStream.Flush();
    }
}
