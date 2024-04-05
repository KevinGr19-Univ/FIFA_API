using FIFA_API.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
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
        private readonly FifaDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Crée une instance de <see cref="PasswordResetService"/>.
        /// </summary>
        /// <param name="context">Le DbContext à utiliser.</param>
        /// <param name="config">La configuration contenant les variables liées à la réinitialisation de mots de passe.</param>
        /// <param name="emailSender">Le service d'envoi de mail à utiliser.</param>
        /// <param name="passwordHasher">Le hasheur de mot de passe à utiliser.</param>
        public PasswordResetService(FifaDbContext context, IConfiguration config, IEmailSender emailSender, IPasswordHasher passwordHasher)
        {
            _context = context;
            _config = config;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async Task SendPasswordResetCodeAsync(string mail)
        {
            var reset = await _context.PasswordResets.FindAsync(mail);

            bool add = reset is null;
            if (add)
            {
                bool exists = await _context.Utilisateurs.IsEmailTaken(mail);
                if (!exists) return;

                reset = new() { Mail = mail };
            }

            reset.Code = GenerateCode();
            reset.Date = DateTime.Now;

            if (add) await _context.PasswordResets.AddAsync(reset);
            else _context.PasswordResets.Update(reset);

            await _context.SaveChangesAsync();
            await _emailSender.SendAsync(
                to: mail,
                subject: "FIFA - Reset your password",
                message: GenerateMessage(mail, reset)
            );
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, string code)
        {
            var reset = await _context.PasswordResets.SingleOrDefaultAsync(r => r.Code == code);
            if (reset is null || reset.Code != code) return false;

            _context.PasswordResets.Remove(reset);
            await _context.SaveChangesAsync();

            var expireMinutes = int.Parse(_config["PasswordReset:ExpireMinutes"]);
            if (reset.Date.AddMinutes(expireMinutes) < DateTime.Now) return false;

            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Mail == request.Mail);
            if (user is null) return false;

            try
            {
                user.HashMotDePasse = _passwordHasher.Hash(request.NewPassword);
                await _context.SaveChangesAsync();
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
