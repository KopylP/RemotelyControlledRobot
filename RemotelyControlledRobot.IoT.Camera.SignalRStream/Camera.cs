using Iot.Device.Media;

namespace RemotelyControlledRobot.IoT.Camera.SignalRStream;

public class Camera : IDisposable
{
    private readonly VideoConnectionSettings _settings = new VideoConnectionSettings(
        busId: 0,
        captureSize: (320, 240),
        pixelFormat: VideoPixelFormat.JPEG
    );

    private readonly VideoDevice _device;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    public event VideoDevice.NewImageBufferReadyEvent NewImageReady
    {
        add => _device.NewImageBufferReady += value;
        remove => _device.NewImageBufferReady -= value;
    }

    public Camera()
    {
        _settings.WhiteBalanceEffect = WhiteBalanceEffect.Daylight;
        _device = VideoDevice.Create(_settings);
        _device.ImageBufferPoolingEnabled = true;
    }

    public void StartCapture()
    {
        if (!_device.IsOpen)
        {
            _device.StartCaptureContinuous();
        }

        if (!_device.IsCapturing)
        {
            new Thread(() =>
            {
                _device.CaptureContinuous(_cancellationTokenSource.Token);
            }).Start();

            Console.WriteLine("Camera was started.");
        }
    }

    public void StopCapture()
    {
        if (_device.IsCapturing)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _device.StopCaptureContinuous();
        }
    }
    
    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
        }

        _cancellationTokenSource.Dispose();
        _device.StopCaptureContinuous();
        _device.Dispose();
    }
}