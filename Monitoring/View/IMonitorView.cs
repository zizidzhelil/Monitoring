using AForge.Controls;
using System.Collections.Generic;

namespace trackk.View
{
   public class IMonitorView
   {
      public IList<string> Cameras { get; set; }

      public VideoSourcePlayer VideoPlayer { get; set; }
   }
}
