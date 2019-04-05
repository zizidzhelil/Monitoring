using AForge.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace Monitoring.Processors
{
   public class ImageProcessor
   {
      private const float compareLevel = 0.98f;

      public bool CompareImages(Bitmap imageOne, Bitmap imageTwo)
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
