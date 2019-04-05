using AForge.Video.DirectShow;
using System;

namespace Monitoring.ComponentsLoader
{
   public class CameraNamesLoader
   {
      private readonly FilterInfoCollection _filterInfo;

      public CameraNamesLoader(FilterInfoCollection filterInfo)
      {
         _filterInfo = filterInfo;
      }

      public string[] Load()
      {
         if (_filterInfo.Count == 0)
            throw new ApplicationException();

         string[] names = new string[_filterInfo.Count];

         for (int i = 0; i < _filterInfo.Count; i++)
         {
            names[i] = _filterInfo[i].Name;
         }

         return names;
      }
   }
}
