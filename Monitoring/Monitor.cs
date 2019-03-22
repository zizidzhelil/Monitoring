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
using System.Diagnostics;

namespace Monitoring
{
   public partial class Monitor : Form
   {
      private const int imageCompareReplacementCount = 10;
      private const float similarityThreshold = 0.5f;
      private const float compareLevel = 0.98f;
      private FilterInfoCollection videoDevices;
      private EuclideanColorFiltering filter = new EuclideanColorFiltering();
      private Color color = Color.Black;
      private GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
      private BlobCounter blobCounter = new BlobCounter();
      private int range = 120;
      private Bitmap bitmapCompare;
      private int frameCounterSinceLastUpdate = 1;

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
         frameCounterSinceLastUpdate++;
         if (bitmapCompare == null || frameCounterSinceLastUpdate % imageCompareReplacementCount == 0)
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} VALUES CHANGED");

            bitmapCompare = (Bitmap)image.Clone();
            return;
         }

         if (!CompareImages(bitmapCompare, (Bitmap)image.Clone()))
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE DIFFERENT");
         }
         else
         {
            Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE SAME");
         }
      }

      public static Boolean CompareImages(Bitmap imageOne, Bitmap imageTwo)
      {
         var newBitmap1 = ChangePixelFormat(new Bitmap(imageOne), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
         var newBitmap2 = ChangePixelFormat(new Bitmap(imageTwo), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

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
         var match = results[0].Similarity >= compareLevel;

         return match;
      }

      private static Bitmap ChangePixelFormat(Bitmap inputImage, System.Drawing.Imaging.PixelFormat newFormat)
      {
         return (inputImage.Clone(new Rectangle(0, 0, inputImage.Width, inputImage.Height), newFormat));
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
