using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Services
{
    public class Login2FAService : ILogin2FAService
    {
        private readonly FifaDbContext _context;
        private readonly IConfiguration _config;
        private readonly ISmsService _smsService;

        public Login2FAService(FifaDbContext context, IConfiguration config, ISmsService smsService)
        {
            _config = config;
            _context = context;
            _smsService = smsService;
        }

        public async Task<bool> Remove2FACode(Utilisateur user)
        {
            var auth2fa = await _context.Login2FAs.FindAsync(user.Id);
            if (auth2fa is null) return false;

            _context.Login2FAs.Remove(auth2fa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> Send2FACodeAsync(Utilisateur user)
        {
            if (user.Telephone is null)
                throw new ArgumentException("User has no phone");

            var auth2fa = await _context.Login2FAs.FindAsync(user.Id);

            bool add = auth2fa is null;
            if (add) auth2fa = new() { IdUtilisateur = user.Id };

            user.Token2FA = GenerateToken();
            auth2fa.Code = GenerateCode();
            auth2fa.Date = DateTime.Now;

            if (add) await _context.Login2FAs.AddAsync(auth2fa);
            await _context.SaveChangesAsync();

            await _smsService.SendSMSAsync(user.Telephone, GenerateMessage(user, auth2fa));
            return user.Token2FA;
        }

        public async Task<Utilisateur?> AuthenticateAsync(string token, string code)
        {
            var user = await _context.Utilisateurs.SingleOrDefaultAsync(u => u.Token2FA == token);
            if (user is null) return null;

            var auth2fa = await _context.Login2FAs.FindAsync(user.Id);
            if (auth2fa is null || auth2fa.Code != code) return null;

            user.Token2FA = null;
            _context.Login2FAs.Remove(auth2fa);
            await _context.SaveChangesAsync();

            var expireMinutes = int.Parse(_config["2FA:ExpireMinutes"]);
            if (auth2fa.Date.AddMinutes(expireMinutes) < DateTime.Now) return null;

            return user;
        }

        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateCode()
        {
            Random r = new Random((int)(DateTime.UtcNow.Ticks * 165487812) + 48995);
            return r.Next(1_000_000).ToString();
        }

        public string GenerateMessage(Utilisateur user, Auth2FALogin auth2fa)
        {
            return $"{auth2fa.Code} - This code will expire in {_config["2FA:ExpireMinutes"]} minutes.";
        }
    }
}
