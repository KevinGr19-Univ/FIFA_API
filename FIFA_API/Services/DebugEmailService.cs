using FIFA_API.Contracts;

namespace FIFA_API.Services
{
    /// <summary>
    /// Classe de substitution pour débogguer l'envoi de mail.
    /// </summary>
    public class DebugEmailService : IEmailSender
    {
        private readonly ILogger<IEmailSender> _logger;

        public DebugEmailService(ILogger<IEmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string message)
        {
            _logger.LogInformation($"[MAIL] To: {to}\nSubject: {subject}\nMessage: {message}");
        }
    }
}
