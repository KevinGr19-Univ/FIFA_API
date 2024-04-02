using FIFA_API.Contracts;
using Microsoft.VisualStudio.Web.CodeGeneration;
using SQLitePCL;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace FIFA_API.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly ILogger<ISmsService>? _logger;
        private readonly string _fromPhoneNumber;
        private readonly string? _overrideTo;

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
