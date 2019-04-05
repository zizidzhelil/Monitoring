using Monitoring.View;
using System;
using System.Windows.Forms;

namespace Monitoring
{
   static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Monitor());
        }
    }
}
