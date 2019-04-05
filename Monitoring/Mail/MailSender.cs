using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Monitoring.Mail
{
   public class MailSender
   {
      private readonly SmtpClient _mailClient;
      private readonly MemoryStream _memoryStream;

      public MailSender()
      {
         _mailClient = new SmtpClient("smtp.gmail.com", 587)
         {
            EnableSsl = true,
            Credentials = new NetworkCredential("zeni.dzhelil97@gmail.com", "a123b456c")
         };
      }

      public bool Send(Bitmap image)
      {
         try
         {
            var message = BuildMessage(image);
            _mailClient.Send(message);

            return true;
         }
         catch
         {
            return false;
         }
         finally
         {
            _memoryStream?.Dispose();
         }
      }

      private MailMessage BuildMessage(Bitmap image)
      {
         image.Save(_memoryStream, ImageFormat.Jpeg);

         var mailMessage = new MessageBuilder()
            .From("zeni.dzhelil97@gmail.com")
            .To("zizidzhelil@gmail.com")
            .Subject("Внимание!")
            .Body("Засечено е движение!")
            .Attachment(_memoryStream, "image.jpg")
            .Build();

         return mailMessage;
      }
   }
}
