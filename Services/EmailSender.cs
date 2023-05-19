using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WebApp.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        private readonly EmailSettings _emailSettings;

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Email, _emailSettings.DisplayName, Encoding.UTF8),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                mail.To.Add(new MailAddress(toEmail));

                using (SmtpClient smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.UseSSL;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
