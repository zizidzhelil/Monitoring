using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;


namespace Monitoring
{
   public partial class Monitor : Form
   {
      private FilterInfoCollection videoDevices;
      EuclideanColorFiltering filter = new EuclideanColorFiltering();
      Color color = Color.Black;
      GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
      BlobCounter blobCounter = new BlobCounter();
      int range = 120;

      public Monitor()
      {
         InitializeComponent();

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
               camerasCombo.Items.Add(device.Name);
            }

            camerasCombo.SelectedIndex = 0;
         }
         catch (ApplicationException)
         {
            camerasCombo.Items.Add("No local capture devices");
            videoDevices = null;
         }
      }

      private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
      {
         Bitmap objectsImage = null;
         Bitmap mImage = null;
         mImage = (Bitmap)image.Clone();
         filter.CenterColor = Color.FromArgb(color.ToArgb());
         filter.Radius = (short)range;

         objectsImage = image;
         filter.ApplyInPlace(objectsImage);

         BitmapData objectsData = objectsImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
         ImageLockMode.ReadOnly, image.PixelFormat);
         UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
         objectsImage.UnlockBits(objectsData);

         blobCounter.ProcessImage(grayImage);

         image = mImage;
      }

      private void button1_Click(object sender, EventArgs e)
      {
         videoSourcePlayer1.SignalToStop();
         videoSourcePlayer1.WaitForStop();

         VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[camerasCombo.SelectedIndex].MonikerString);
         videoSource.DesiredFrameSize = new Size(320, 240);
         videoSource.DesiredFrameRate = 2;

         videoSourcePlayer1.VideoSource = videoSource;
         videoSourcePlayer1.Start();
      }

      private void f21_FormClosing(object sender, FormClosingEventArgs e)
      {
         videoSourcePlayer1.SignalToStop();
         videoSourcePlayer1.WaitForStop();
         groupBox1.Enabled = true;
      }

      private void button2_Click(object sender, EventArgs e)
      {
         videoSourcePlayer1.SignalToStop();
         videoSourcePlayer1.WaitForStop();
      }
   }
}
