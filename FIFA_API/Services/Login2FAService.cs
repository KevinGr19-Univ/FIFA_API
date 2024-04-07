using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Services
{
    /// <summary>
    /// Service de 2FA par SMS.
    /// </summary>
    public class Login2FAService : ILogin2FAService
    {
        private readonly IUnitOfWorkUserServices _uow;
        private readonly IConfiguration _config;
        private readonly ISmsService _smsService;

        /// <summary>
        /// Crée une instance de <see cref="Login2FAService"/>.
        /// </summary>
        /// <param name="uow">Le UoW à utiliser.</param>
        /// <param name="config">La configuration contenant les variables liées à la 2FA.</param>
        /// <param name="smsService">Le service d'envoi de SMS à utiliser.</param>
        public Login2FAService(IUnitOfWorkUserServices uow, IConfiguration config, ISmsService smsService)
        {
            _config = config;
            _uow = uow;
            _smsService = smsService;
        }

        public async Task<bool> Remove2FACode(Utilisateur user)
        {
            var auth2fa = await _uow.Login2FAs.GetById(user.Id);
            if (auth2fa is null) return false;

            await _uow.Login2FAs.Delete(auth2fa);
            await _uow.SaveChanges();
            return true;
        }

        public async Task<string> Send2FACodeAsync(Utilisateur user)
        {
            if (user.Telephone is null)
                throw new ArgumentException("User has no phone");

            var auth2fa = await _uow.Login2FAs.GetById(user.Id);

            bool add = auth2fa is null;
            if (add) auth2fa = new() { IdUtilisateur = user.Id };

            user.Token2FA = GenerateToken();
            auth2fa.Code = GenerateCode();
            auth2fa.Date = DateTime.Now;

            if (add) await _uow.Login2FAs.Add(auth2fa);
            await _uow.SaveChanges();

            await _smsService.SendSMSAsync(user.Telephone, GenerateMessage(user, auth2fa));
            return user.Token2FA;
        }

        public async Task<Utilisateur?> AuthenticateAsync(string token, string code)
        {
            var user = await _uow.Utilisateurs.GetBy2FAToken(token);
            if (user is null) return null;

            var auth2fa = await _uow.Login2FAs.GetById(user.Id);
            if (auth2fa is null || auth2fa.Code != code) return null;

            user.Token2FA = null;
            await _uow.Login2FAs.Delete(auth2fa);
            await _uow.SaveChanges();

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
