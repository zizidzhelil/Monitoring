namespace Monitoring.View
{
   partial class Monitor
   {
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.CamerasCollection = new System.Windows.Forms.ComboBox();
         this.BtnStart = new System.Windows.Forms.Button();
         this.BtnDisconnect = new System.Windows.Forms.Button();
         this.CameraControlsContainer = new System.Windows.Forms.GroupBox();
         this.LblInfo = new System.Windows.Forms.Label();
         this.VideoPlayer = new AForge.Controls.VideoSourcePlayer();
         this.VidePlayerContainer = new System.Windows.Forms.GroupBox();
         this.CameraControlsContainer.SuspendLayout();
         this.VidePlayerContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // CamerasCollection
         // 
         this.CamerasCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.CamerasCollection.FormattingEnabled = true;
         this.CamerasCollection.Location = new System.Drawing.Point(18, 26);
         this.CamerasCollection.Name = "CamerasCollection";
         this.CamerasCollection.Size = new System.Drawing.Size(167, 21);
         this.CamerasCollection.TabIndex = 5;
         // 
         // BtnStart
         // 
         this.BtnStart.BackColor = System.Drawing.SystemColors.ActiveCaption;
         this.BtnStart.Location = new System.Drawing.Point(18, 53);
         this.BtnStart.Name = "BtnStart";
         this.BtnStart.Size = new System.Drawing.Size(75, 23);
         this.BtnStart.TabIndex = 6;
         this.BtnStart.Text = "Start";
         this.BtnStart.UseVisualStyleBackColor = false;
         this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
         // 
         // BtnDisconnect
         // 
         this.BtnDisconnect.BackColor = System.Drawing.SystemColors.ActiveCaption;
         this.BtnDisconnect.Location = new System.Drawing.Point(110, 53);
         this.BtnDisconnect.Name = "BtnDisconnect";
         this.BtnDisconnect.Size = new System.Drawing.Size(75, 23);
         this.BtnDisconnect.TabIndex = 10;
         this.BtnDisconnect.Text = "Disconnect";
         this.BtnDisconnect.UseVisualStyleBackColor = false;
         this.BtnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
         // 
         // CameraControlsContainer
         // 
         this.CameraControlsContainer.BackColor = System.Drawing.SystemColors.ButtonFace;
         this.CameraControlsContainer.Controls.Add(this.LblInfo);
         this.CameraControlsContainer.Controls.Add(this.BtnStart);
         this.CameraControlsContainer.Controls.Add(this.BtnDisconnect);
         this.CameraControlsContainer.Controls.Add(this.CamerasCollection);
         this.CameraControlsContainer.Location = new System.Drawing.Point(829, 12);
         this.CameraControlsContainer.Name = "CameraControlsContainer";
         this.CameraControlsContainer.Size = new System.Drawing.Size(207, 165);
         this.CameraControlsContainer.TabIndex = 11;
         this.CameraControlsContainer.TabStop = false;
         this.CameraControlsContainer.Text = "Connection";
         // 
         // LblInfo
         // 
         this.LblInfo.AutoSize = true;
         this.LblInfo.Location = new System.Drawing.Point(88, 119);
         this.LblInfo.Name = "LblInfo";
         this.LblInfo.Size = new System.Drawing.Size(0, 13);
         this.LblInfo.TabIndex = 11;
         // 
         // VideoPlayer
         // 
         this.VideoPlayer.Location = new System.Drawing.Point(11, 13);
         this.VideoPlayer.Name = "VideoPlayer";
         this.VideoPlayer.Size = new System.Drawing.Size(795, 729);
         this.VideoPlayer.TabIndex = 3;
         this.VideoPlayer.Text = "Video Player";
         this.VideoPlayer.VideoSource = null;
         this.VideoPlayer.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.VideoPlayer_NewFrame);
         // 
         // VidePlayerContainer
         // 
         this.VidePlayerContainer.Controls.Add(this.VideoPlayer);
         this.VidePlayerContainer.Location = new System.Drawing.Point(1, -1);
         this.VidePlayerContainer.Name = "VidePlayerContainer";
         this.VidePlayerContainer.Size = new System.Drawing.Size(822, 804);
         this.VidePlayerContainer.TabIndex = 13;
         this.VidePlayerContainer.TabStop = false;
         this.VidePlayerContainer.Text = "View";
         // 
         // Monitor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ButtonFace;
         this.ClientSize = new System.Drawing.Size(1048, 749);
         this.Controls.Add(this.CameraControlsContainer);
         this.Controls.Add(this.VidePlayerContainer);
         this.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Name = "Monitor";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Monitoring";
         this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Monitoring_FormClosing);
         this.CameraControlsContainer.ResumeLayout(false);
         this.CameraControlsContainer.PerformLayout();
         this.VidePlayerContainer.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.ComboBox CamerasCollection;
      private System.Windows.Forms.Button BtnStart;
      private System.Windows.Forms.Button BtnDisconnect;
      private System.Windows.Forms.GroupBox CameraControlsContainer;
      private System.Windows.Forms.Label LblInfo;
      private AForge.Controls.VideoSourcePlayer VideoPlayer;
      private System.Windows.Forms.GroupBox VidePlayerContainer;
   }
}

