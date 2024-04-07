using FIFA_API.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FIFA_API.Services
{
    /// <summary>
    /// Service de réinitialisation de mots de passe.
    /// </summary>
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUnitOfWorkUserServices _uow;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Crée une instance de <see cref="PasswordResetService"/>.
        /// </summary>
        /// <param name="uow">Le UoW à utiliser.</param>
        /// <param name="config">La configuration contenant les variables liées à la réinitialisation de mots de passe.</param>
        /// <param name="emailSender">Le service d'envoi de mail à utiliser.</param>
        /// <param name="passwordHasher">Le hasheur de mot de passe à utiliser.</param>
        public PasswordResetService(IUnitOfWorkUserServices uow, IConfiguration config, IEmailSender emailSender, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _config = config;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async Task SendPasswordResetCodeAsync(string mail)
        {
            var reset = await _uow.PasswordResets.GetById(mail);

            bool add = reset is null;
            if (add)
            {
                bool exists = await _uow.Utilisateurs.IsEmailTaken(mail);
                if (!exists) return;

                reset = new() { Mail = mail };
            }

            reset.Code = GenerateCode();
            reset.Date = DateTime.Now;

            if (add) await _uow.PasswordResets.Add(reset);
            else await _uow.PasswordResets.Update(reset);

            await _uow.SaveChanges();
            await _emailSender.SendAsync(
                to: mail,
                subject: "FIFA - Reset your password",
                message: GenerateMessage(mail, reset)
            );
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, string code)
        {
            var reset = await _uow.PasswordResets.GetByCode(code);
            if (reset is null || reset.Code != code) return false;

            await _uow.PasswordResets.Delete(reset);
            await _uow.SaveChanges();

            var expireMinutes = int.Parse(_config["PasswordReset:ExpireMinutes"]);
            if (reset.Date.AddMinutes(expireMinutes) < DateTime.Now) return false;

            var user = await _uow.Utilisateurs.GetByEmail(request.Mail);
            if (user is null) return false;

            try
            {
                user.HashMotDePasse = _passwordHasher.Hash(request.NewPassword);
                await _uow.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private string GenerateCode()
        {
            return Guid.NewGuid().ToString();
        }

        private string GenerateMessage(string mail, AuthPasswordReset reset)
        {
            return $"<p>Reset your password by clicking the link below :</p>" +
                $"<a href='{_config["PasswordReset:Url"]}?code={reset.Code}'>Reset your password</a>" +
                $"<br><p>This link is valid for {_config["PasswordReset:ExpireMinutes"]} minutes.</p>";
        }
    }
}
