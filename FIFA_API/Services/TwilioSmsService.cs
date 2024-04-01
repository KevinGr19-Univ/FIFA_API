using FIFA_API.Contracts;
using SQLitePCL;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace FIFA_API.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _fromPhoneNumber;
        private readonly string? _overrideTo;

        public TwilioSmsService(string fromPhoneNumber, string? overrideTo = null)
        {
            _fromPhoneNumber = fromPhoneNumber;

            if (string.IsNullOrWhiteSpace(overrideTo)) overrideTo = null;
            _overrideTo = overrideTo;
        }

        public async Task SendSMSAsync(string to, string message)
        {
            if (_overrideTo is null)
            {
                if (to.Length == 10 && !to.StartsWith('+'))
                    to = $"+33{to}";
            }
            else to = _overrideTo;

            var res = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                to: to,
                body: message
            );
        }
    }
}
