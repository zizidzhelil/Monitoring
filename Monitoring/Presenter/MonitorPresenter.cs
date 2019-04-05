using AForge.Video.DirectShow;
using Monitoring.ComponentsLoader;
using Monitoring.Mail;
using Monitoring.Processors;
using Monitoring.View;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Monitoring.Presenter
{
   public class MonitorPresenter
   {
      private const int InitialWaitTime = 3;

      private readonly IMonitorView _view;
      private readonly MailSender _mailSender;
      private readonly CameraNamesLoader _cameraNamesLoader;
      private readonly VideoCaptureDeviceLoader _videoCaptureDeviceLoader;
      private readonly FilterInfoCollection _videoDevices;
      private readonly ImageProcessor _imageProcessor;

      private Timer startCountdown;
      private int counter = InitialWaitTime;
      private Bitmap bitmapCompare;

      public MonitorPresenter(IMonitorView view)
      {
         _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
         _view = view;
         _mailSender = new MailSender();
         _cameraNamesLoader = new CameraNamesLoader(_videoDevices);
         _videoCaptureDeviceLoader = new VideoCaptureDeviceLoader(_videoDevices);
         _view.Cameras.AddRange(_cameraNamesLoader.Load());
         _view.SelectedCameraIndex = 0;
         _imageProcessor = new ImageProcessor();
      }

      public void OnNewFrame(ref Bitmap image)
      {
         var clonedImage = (Bitmap)image.Clone();

         if (_imageProcessor.UpdateBitmapCompareIfNecessary(ref bitmapCompare, clonedImage)) return;
         if (!_imageProcessor.CompareImages(bitmapCompare, clonedImage))
         {
            _mailSender.Send(clonedImage);
         }
      }

      public void OnBtnStartClick()
      {
         startCountdown = new Timer();
         startCountdown.Tick += new EventHandler(Timer_Tick);
         startCountdown.Interval = 1000;
         startCountdown.Start();

         _view.LblInfoContent = counter.ToString();
      }

      public void OnBtnDisconnectClick()
      {
         _view.VideoPlayerControl.SignalToStop();
         _view.VideoPlayerControl.WaitForStop();

         counter = InitialWaitTime;
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
            _view.VideoPlayerControl.VideoSource = _videoCaptureDeviceLoader.Load(_view.SelectedCameraIndex);
            _view.VideoPlayerControl.Start();
         }

         _view.LblInfoContent = counter.ToString();
      }
   }
}
