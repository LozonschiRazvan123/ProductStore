using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("razvanlozonschi123@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("razvanlozonschi123@gmail.com", "razvan123!!RR");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
