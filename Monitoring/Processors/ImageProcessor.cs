using AForge.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace Monitoring.Processors
{
   public class ImageProcessor
   {
      private const float compareLevel = 0.98f;
      private const int imageCompareReplacementCount = 10;

      private int frameCounterSinceLastUpdate = 1;

      public bool UpdateBitmapCompareIfNecessary(ref Bitmap bitmapCompare, Bitmap currentImage)
      {
         frameCounterSinceLastUpdate++;
         if (bitmapCompare == null || frameCounterSinceLastUpdate % imageCompareReplacementCount == 0)
         {
            bitmapCompare = currentImage;
            return true;
         }

         return false;
      }

      public bool CompareImages(Bitmap imageOne, Bitmap imageTwo)
      {
         var newBitmap1 = ChangePixelFormat(new Bitmap(imageOne), PixelFormat.Format24bppRgb);
         var newBitmap2 = ChangePixelFormat(new Bitmap(imageTwo), PixelFormat.Format24bppRgb);

         var tm = new ExhaustiveTemplateMatching();

         var results = tm.ProcessImage(newBitmap1, newBitmap2);

         if (results.Length <= 0)
         {
            return false;
         }

         return results[0].Similarity >= compareLevel;
      }

      private Bitmap ChangePixelFormat(Bitmap inputImage, PixelFormat newFormat)
      {
         return inputImage.Clone(new Rectangle(0, 0, inputImage.Width, inputImage.Height), newFormat);
      }
   }
}
