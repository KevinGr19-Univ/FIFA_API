using FIFA_API.Contracts;
using Microsoft.VisualStudio.Web.CodeGeneration;
using SQLitePCL;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace FIFA_API.Services
{
    /// <summary>
    /// Service d'envoi de SMS par l'API Twilio.
    /// </summary>
    public class TwilioSmsService : ISmsService
    {
        private readonly ILogger<ISmsService>? _logger;
        private readonly string _fromPhoneNumber;
        private readonly string? _overrideTo;

        /// <summary>
        /// Crée une instance de <see cref="TwilioSmsService"/>.
        /// </summary>
        /// <param name="fromPhoneNumber">Le numéro de téléphone à utiliser.</param>
        /// <param name="overrideTo">Le numéro de téléphone par lequel remplacer le destinataire (déboggage).</param>
        /// <param name="logger">Logger pour les messages d'erreurs.</param>
        public TwilioSmsService(string fromPhoneNumber, string? overrideTo = null, ILogger<ISmsService>? logger = null)
        {
            _fromPhoneNumber = fromPhoneNumber;

            if (string.IsNullOrWhiteSpace(overrideTo)) overrideTo = null;
            _overrideTo = overrideTo;

            _logger = logger;
        }

        public async Task SendSMSAsync(string to, string message)
        {
            if (_overrideTo is null)
            {
                if (to.Length == 10 && !to.StartsWith('+'))
                    to = $"+33{to}";
            }
            else to = _overrideTo;

            try
            {
                var res = await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                    to: to,
                    body: message
                );
            }
            catch (Exception e)
            {
                _logger?.LogError(exception: e, message: $"Could not send SMS from {_fromPhoneNumber} to {to}");
            }
        }
    }
}
