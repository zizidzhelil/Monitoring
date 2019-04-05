using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using Monitoring.View;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Monitoring.Presenter
{
   public class MonitorPresenter
   {
      private Timer timer1;
      private int counter = 3;

      private readonly IMonitorView _view;

      private const int imageCompareReplacementCount = 10;
      private const float similarityThreshold = 0.5f;
      private const float compareLevel = 0.98f;

      private readonly FilterInfoCollection videoDevices;
      private readonly EuclideanColorFiltering filter = new EuclideanColorFiltering();
      private readonly Color color = Color.Black;
      private readonly GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
      private readonly BlobCounter blobCounter = new BlobCounter();

      private Bitmap bitmapCompare;
      private int frameCounterSinceLastUpdate = 1;

      public MonitorPresenter(IMonitorView view)
      {
         this._view = view;

         blobCounter.MinWidth = 2;
         blobCounter.MinHeight = 2;
         blobCounter.FilterBlobs = true;
         blobCounter.ObjectsOrder = ObjectsOrder.Size;

         try
         {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
               throw new ApplicationException();

            foreach (FilterInfo device in videoDevices)
            {
               _view.Cameras.Add(device.Name);
            }

            _view.SelectedCameraIndex = 0;
         }
         catch
         {
            _view.Cameras.Add("No local capture devices");
            videoDevices = null;
         }
      }

      public void OnNewFrame(ref Bitmap image)
      {
         frameCounterSinceLastUpdate++;
         if (bitmapCompare == null || frameCounterSinceLastUpdate % imageCompareReplacementCount == 0)
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} VALUES CHANGED");

            bitmapCompare = (Bitmap)image.Clone();
            return;
         }

         if (!CompareImages(bitmapCompare, (Bitmap)image.Clone()))
         {
            try
            {
               MailMessage mail = new MailMessage();

               mail.From = new MailAddress("zeni.dzhelil97@gmail.com");
               mail.To.Add("zizidzhelil@gmail.com");
               mail.Subject = "Внимание!";
               mail.Body = "Засечено е движение!";

               SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587)
               {
                  EnableSsl = true,
                  Credentials = new NetworkCredential("zeni.dzhelil97@gmail.com", "a123b456c")
               };

               using (var memoryStream = new MemoryStream())
               {
                  image.Save("image.png", ImageFormat.Png);

                  var imageAttachment = (Bitmap)image.Clone();
                  imageAttachment.Save(memoryStream, ImageFormat.Jpeg);
                  memoryStream.Position = 0;
                  var attachment = new Attachment(memoryStream, "image.jpeg");
                  mail.Attachments.Add(attachment);

                  SmtpServer.Send(mail);
               }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }

            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE DIFFERENT");
         }
         else
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE SAME");
         }
      }

      public void OnBtnStartClick()
      {
         timer1 = new Timer();
         timer1.Tick += new EventHandler(Timer_Tick);
         timer1.Interval = 1000; // 1 second
         timer1.Start();

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
            timer1.Stop();

            _view.VideoPlayerControl.SignalToStop();
            _view.VideoPlayerControl.WaitForStop();

            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[_view.SelectedCameraIndex].MonikerString)
            {
               DesiredFrameSize = new Size(320, 240),
               DesiredFrameRate = 2
            };

            _view.VideoPlayerControl.VideoSource = videoSource;
            _view.VideoPlayerControl.Start();
         }

         _view.LblInfoContent = counter.ToString();
      }

      private Boolean CompareImages(Bitmap imageOne, Bitmap imageTwo)
      {
         var newBitmap1 = ChangePixelFormat(new Bitmap(imageOne), PixelFormat.Format24bppRgb);
         var newBitmap2 = ChangePixelFormat(new Bitmap(imageTwo), PixelFormat.Format24bppRgb);

         // Setup the AForge library
         var tm = new ExhaustiveTemplateMatching();

         // Process the images
         var results = tm.ProcessImage(newBitmap1, newBitmap2);

         // Compare the results, 0 indicates no match so return false
         if (results.Length <= 0)
         {
            return false;
         }

         // Return true if similarity score is equal or greater than the comparison level
         return results[0].Similarity >= compareLevel;
      }

      private Bitmap ChangePixelFormat(Bitmap inputImage, PixelFormat newFormat)
      {
         return inputImage.Clone(new Rectangle(0, 0, inputImage.Width, inputImage.Height), newFormat);
      }
   }
}
