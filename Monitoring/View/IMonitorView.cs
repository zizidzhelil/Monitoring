using AForge.Controls;
using System.Collections.Generic;
using static System.Windows.Forms.ComboBox;

namespace Monitoring.View
{
   public interface IMonitorView
   {
      ObjectCollection Cameras { get; }

      int SelectedCameraIndex { get; set; }

      bool IsCameraControlActive { get; set; }

      VideoSourcePlayer VideoPlayerControl { get; set; }

      string LblInfoContent { get; set; }
   }
}
