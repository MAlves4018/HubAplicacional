using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail()
        {
            string body = "A app caiu.";
            var email = new MimeMessage(body);
            email.From.Add(MailboxAddress.Parse("lexus98@ethereal.email"));
            // email.To.Add(MailboxAddress.Parse("hubaplicacionalassistent@gmail.com"));
            email.To.Add(MailboxAddress.Parse("lexus98@ethereal.email"));
            email.Subject = "Teste email subject";
            email.Body= new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com",587,SecureSocketOptions.StartTls);
            smtp.Connect("smtp.ethereal.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("lexus98@ethereal.email", "Fph1Mj9DkCKhx3CcQA");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
