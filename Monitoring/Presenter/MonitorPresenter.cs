using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using Monitoring.ComponentsLoader;
using Monitoring.Mail;
using Monitoring.Processors;
using Monitoring.View;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Monitoring.Presenter
{
   public class MonitorPresenter
   {
      private Timer startCountdown;
      private int counter = 3;

      private readonly IMonitorView _view;
      private readonly MailSender _mailSender;
      private readonly CameraNamesLoader _cameraNamesLoader;
      private readonly FilterInfoCollection _videoDevices;
      private readonly ImageProcessor _imageProcessor;

      private const int imageCompareReplacementCount = 10;
      private const float similarityThreshold = 0.5f;


      private readonly EuclideanColorFiltering filter = new EuclideanColorFiltering();
      private readonly Color color = Color.Black;
      private readonly GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();

      private Bitmap bitmapCompare;
      private int frameCounterSinceLastUpdate = 1;

      public MonitorPresenter(IMonitorView view)
      {
         _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
         _view = view;
         _mailSender = new MailSender();
         _cameraNamesLoader = new CameraNamesLoader(_videoDevices);
         _view.Cameras.AddRange(_cameraNamesLoader.Load());
         _view.SelectedCameraIndex = 0;
         _imageProcessor = new ImageProcessor();
      }

      public void OnNewFrame(ref Bitmap image)
      {
         var clonedImage = (Bitmap)image.Clone();
         frameCounterSinceLastUpdate++;
         if (bitmapCompare == null || frameCounterSinceLastUpdate % imageCompareReplacementCount == 0)
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} VALUES CHANGED");

            bitmapCompare = (Bitmap)image.Clone();
            return;
         }

         if (!_imageProcessor.CompareImages(bitmapCompare, (Bitmap)image.Clone()))
         {
            _mailSender.Send(clonedImage);

            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE DIFFERENT");
         }
         else
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE SAME");
         }
      }

      public void OnBtnStartClick()
      {
         startCountdown = new Timer();
         startCountdown.Tick += new EventHandler(Timer_Tick);
         startCountdown.Interval = 1000; // 1 second
         startCountdown.Start();

         _view.LblInfoContent = counter.ToString();
      }

      public void OnBtnDisconnectClick()
      {
         _view.VideoPlayerControl.SignalToStop();
         _view.VideoPlayerControl.WaitForStop();
      }

      public void OnMonitoringFormClosing()
      {
         _view.VideoPlayerControl.SignalToStop();
         _view.VideoPlayerControl.WaitForStop();
         _view.IsCameraControlActive = true;
      }

      private void Timer_Tick(object sender, EventArgs e)
      {
         counter--;
         if (counter == 0)
         {
            startCountdown.Stop();

            _view.VideoPlayerControl.SignalToStop();
            _view.VideoPlayerControl.WaitForStop();

            VideoCaptureDevice videoSource = new VideoCaptureDevice(_videoDevices[_view.SelectedCameraIndex].MonikerString)
            {
               DesiredFrameSize = new Size(320, 240),
               DesiredFrameRate = 2
            };

            _view.VideoPlayerControl.VideoSource = videoSource;
            _view.VideoPlayerControl.Start();
         }

         _view.LblInfoContent = counter.ToString();
      }
   }
}
