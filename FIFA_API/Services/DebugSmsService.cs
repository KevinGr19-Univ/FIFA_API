using FIFA_API.Contracts;

namespace FIFA_API.Services
{
    /// <summary>
    /// Classe de substitution pour débogguer l'envoi de SMS.
    /// </summary>
    public class DebugSmsService : ISmsService
    {
        private readonly ILogger<ISmsService> _logger;

        public DebugSmsService(ILogger<ISmsService> logger)
        {
            _logger = logger;
        }

        public async Task SendSMSAsync(string to, string message)
        {
            _logger.LogInformation($"[SMS] To: {to}\nMessage: {message}");
        }
    }
}
