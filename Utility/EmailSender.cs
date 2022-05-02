using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;

namespace Rock.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("", ""));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("", 25, false);
                await client.AuthenticateAsync("", "");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
