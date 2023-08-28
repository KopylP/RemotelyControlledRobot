using System.Buffers;
using Iot.Device.Media;

namespace RemotelyControlledRobot.Camera.Framework
{
    public class Camera : IDisposable
    {
        private readonly VideoConnectionSettings _settings = new VideoConnectionSettings(
                busId: 0,
                captureSize: (640, 480),
                pixelFormat: VideoPixelFormat.JPEG
                );

        private readonly VideoDevice _device;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly IList<VideoDevice.NewImageBufferReadyEvent> _subscribers = new List<VideoDevice.NewImageBufferReadyEvent>();

        public event VideoDevice.NewImageBufferReadyEvent NewImageReady
        {
            add
            {
                _subscribers.Add(value);
            }
            remove
            {
                _subscribers.Remove(value);
            }
        }

        public Camera()
        {
            _settings.WhiteBalanceEffect = WhiteBalanceEffect.Daylight;
            _device = VideoDevice.Create(_settings);
            _device.ImageBufferPoolingEnabled = true;
            _device.NewImageBufferReady += Device_NewImageBufferReady;
        }

        public void StartCapture()
        {
            Console.WriteLine("Starting camera device...");
            Console.WriteLine("Waiting 20 seconds before camera is calibrated...");
            Thread.Sleep(20_000);

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

        private void Device_NewImageBufferReady(object sender, NewImageBufferReadyEventArgs e)
        {
            _subscribers.AsParallel().ForAll(@event =>
            {
                @event.Invoke(sender, e);
            });

            ArrayPool<byte>.Shared.Return(e.ImageBuffer);
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
}