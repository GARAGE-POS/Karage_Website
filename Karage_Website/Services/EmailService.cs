using System.Net;
using System.Net.Mail;
using Karage_Website.Models;
using Microsoft.Extensions.Options;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public void SendEmail(string subject, string body)
    {
        using (var client = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.SmtpPort))
        {
            client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(_smtpSettings.To);

            client.Send(mailMessage);
        }
    }
}
