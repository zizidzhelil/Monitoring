using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Drawing.Imaging;
using System.IO;

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
                try
                {
                    MailMessage mail = new MailMessage();

                    mail.From = new MailAddress("zeni.dzhelil97@gmail.com");
                    mail.To.Add("zizidzhelil@gmail.com");
                    mail.Subject = "Внимание!";
                    mail.Body = "Засечено е движение!";

                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential("zeni.dzhelil97@gmail.com", "a123b456c")
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        image.Save("image.png", ImageFormat.Png);

                        var imageAttachment = (Bitmap)image.Clone();
                        imageAttachment.Save(memoryStream, ImageFormat.Jpeg);
                        memoryStream.Position = 0;
                        var attachment = new Attachment(memoryStream, "image.jpeg");
                        mail.Attachments.Add(attachment);

                        SmtpServer.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE DIFFERENT");
            }
            else
            {
                Debug.WriteLine($"{Guid.NewGuid().ToString()} THEY ARE SAME");
            }
        }

        public static Boolean CompareImages(Bitmap imageOne, Bitmap imageTwo)
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

        private static Bitmap ChangePixelFormat(Bitmap inputImage, PixelFormat newFormat)
        {
            return inputImage.Clone(new Rectangle(0, 0, inputImage.Width, inputImage.Height), newFormat);
        }

        private Timer timer1;
        private int counter = 3;

        private void Button1_Click(object sender, EventArgs e)
        {           
            timer1 = new Timer();
            timer1.Tick += new EventHandler(Timer_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            label1.Text = counter.ToString();         
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                timer1.Stop();

                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.WaitForStop();

                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[camerasCombo.SelectedIndex].MonikerString)
                {
                    DesiredFrameSize = new Size(320, 240),
                    DesiredFrameRate = 2
                };

                videoSourcePlayer.VideoSource = videoSource;
                videoSourcePlayer.Start();
            }

            label1.Text = counter.ToString();
        }

        private void f21_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
            groupBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}