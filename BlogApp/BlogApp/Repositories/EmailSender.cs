using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace BlogApp.Repositories
{
    public class EmailSender : IEmailSender
    {
        public bool SendEmailAsync(string email, string message)
        {
            MailMessage mailMessage = new();
            SmtpClient smtpClient = new();
            mailMessage.From = new MailAddress("yourFrom@email.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = "Confirm Your Email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<a href='" + message + "'> Click here to confirm your email</a>";

            smtpClient.Port = 587;
            smtpClient.Host = "sandbox.smtp.mailtrap.io";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("7d26d05cc8d3b1", "6c40aca1865fe6");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(mailMessage);
            return true;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mailMessage = new();
            SmtpClient smtpClient = new();
            mailMessage.From = new MailAddress("noReply@email.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<a href='" + htmlMessage + "'> Click here to confirm your email</a>";

            smtpClient.Port = 587;
            smtpClient.Host = "sandbox.smtp.mailtrap.io";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("7d26d05cc8d3b1", "6c40aca1865fe6");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
