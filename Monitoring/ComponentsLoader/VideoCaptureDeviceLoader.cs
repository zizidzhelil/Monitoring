using AForge.Video.DirectShow;
using System.Drawing;

namespace Monitoring.ComponentsLoader
{
   public class VideoCaptureDeviceLoader
   {
      private readonly FilterInfoCollection _videoDevices;

      public VideoCaptureDeviceLoader(FilterInfoCollection videDevices)
      {
         _videoDevices = videDevices;
      }

      public VideoCaptureDevice Load(int selectedCameraIndex)
      {
         return new VideoCaptureDevice(_videoDevices[selectedCameraIndex].MonikerString)
         {
            DesiredFrameSize = new Size(320, 240),
            DesiredFrameRate = 2
         };
      }
   }
}
