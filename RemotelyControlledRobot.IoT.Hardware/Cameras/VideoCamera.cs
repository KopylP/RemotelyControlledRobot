using System.Buffers;
using System.Device.Gpio;
using Iot.Device.Media;
using RemotelyControlledRobot.Framework.System.Abstractions.Hardware;

namespace RemotelyControlledRobot.IoT.Hardware.Cameras;

public class VideoCamera : IHardware, IDisposable
{
    private readonly VideoDevice _device;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public VideoCamera(VideoCameraSettings settings)
    {
        var videoSettings = new VideoConnectionSettings(
            busId: settings.BusId,
            captureSize: settings.CaptureSize,
            pixelFormat: settings.PixelFormat
        )
        {
            WhiteBalanceEffect = settings.WhiteBalanceEffect
        };

        _device = VideoDevice.Create(videoSettings);
        _device.ImageBufferPoolingEnabled = true;
    }

    public void Initialize(GpioController gpioController)
    {
        
        StartCapture();
    }

    public void Stop(GpioController gpioController)
    {
        StopCapture();
    }

    public void CaptureContinuous(CancellationToken cancellationToken)
        => _device.CaptureContinuous(cancellationToken); 


    public void Dispose()
    {
        StopCapture();
        _device.Dispose();
        _cancellationTokenSource.Dispose();
    }

    public event VideoDevice.NewImageBufferReadyEvent NewImageBufferReadyEvent
    {
        add => _device.NewImageBufferReady += value;
        remove => _device.NewImageBufferReady -= value;
    }
    
    private void StartCapture()
    {
        if (_device.IsCapturing) return;
        _device.StartCaptureContinuous();
    }

    private void StopCapture()
    {
        if (!_device.IsCapturing) return;
        _cancellationTokenSource.Cancel();
        _device.StopCaptureContinuous();
    }
}

public class VideoCameraSettings
{
    public static string Section => "VideoCamera";
    
    public int BusId { get; set; } = 0;
    public (uint Width, uint Height) CaptureSize { get; set; } = (320, 240);
    public VideoPixelFormat PixelFormat { get; set; } = VideoPixelFormat.JPEG;
    public WhiteBalanceEffect WhiteBalanceEffect { get; set; } = WhiteBalanceEffect.Daylight;
}