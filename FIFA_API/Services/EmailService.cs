using FIFA_API.Contracts;
using System.Net;
using System.Net.Mail;

namespace FIFA_API.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public Task SendAsync(string to, string subject, string message)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);

            var mail = _config["Smtp:Mail"];
            var username = _config["Smtp:Username"];
            var pwd = _config["Smtp:Password"];

            var smtp = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, pwd)
            };

            var msg = new MailMessage(mail, to, subject, message)
            {
                IsBodyHtml = true
            };
            return smtp.SendMailAsync(msg);
        }
    }
}
