using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;

namespace Monitoring.Mail
{
   public class MessageBuilder
   {
      MailMessage _mailMessage;

      public MessageBuilder()
      {
         _mailMessage = new MailMessage();
      }

      public MessageBuilder From(string from)
      {
         _mailMessage.From = new MailAddress(from);
         return this;
      }

      public MessageBuilder To(string to)
      {
         _mailMessage.To.Add(to);
         return this;
      }

      public MessageBuilder Subject(string subject)
      {
         _mailMessage.Subject = subject;
         return this;
      }

      public MessageBuilder Attachment(MemoryStream content, string name)
      {
         content.Position = 0;

         var attachment = new Attachment(content, name);
         _mailMessage.Attachments.Add(attachment);

         return this;
      }

      public MessageBuilder Body(string body)
      {
         _mailMessage.Body = body;
         return this;
      }

      public MailMessage Build()
      {
         return _mailMessage;
      }
   }
}
