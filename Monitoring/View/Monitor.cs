using AForge.Controls;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using Monitoring.Presenter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using static System.Windows.Forms.ComboBox;

namespace Monitoring.View
{
   public partial class Monitor : Form, IMonitorView
   {
      private MonitorPresenter _presenter;

      public ObjectCollection Cameras
      {
         get { return this.CamerasCollection.Items; }
      }

      public int SelectedCameraIndex
      {
         get { return this.CamerasCollection.SelectedIndex; }
         set { this.CamerasCollection.SelectedIndex = value; }
      }

      public bool IsCameraControlActive
      {
         get { return this.CameraControlsContainer.Enabled; }
         set { this.CameraControlsContainer.Enabled = value; }
      }

      public VideoSourcePlayer VideoPlayerControl
      {
         get { return this.VideoPlayer; }
         set { this.VideoPlayer = value; }
      }

      public string LblInfoContent
      {
         get { return this.LblInfo.Text; }
         set { this.LblInfo.Text = value; }
      }

      public Monitor()
      {
         InitializeComponent();

         _presenter = new MonitorPresenter(this);
      }

      private void VideoPlayer_NewFrame(object sender, ref Bitmap image)
      {
         _presenter.OnNewFrame(ref image);
      }

      private void BtnStart_Click(object sender, EventArgs e)
      {
         _presenter.OnBtnStartClick();
      }

      private void BtnDisconnect_Click(object sender, EventArgs e)
      {
         _presenter.OnBtnDisconnectClick();
      }

      private void Monitoring_FormClosing(object sender, FormClosingEventArgs e)
      {
         _presenter.OnMonitoringFormClosing();
      }
   }
}