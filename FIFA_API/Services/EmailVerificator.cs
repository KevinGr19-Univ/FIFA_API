using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Services
{
    /// <summary>
    /// Service de vérification des adresses mail des utilisateurs.
    /// </summary>
    public class EmailVerificator : IEmailVerificator
    {
        private readonly IUnitOfWorkUserServices _uow;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        /// <summary>
        /// Crée une instance de <see cref="EmailVerificator"/>.
        /// </summary>
        /// <param name="uow">Le UoW à utiliser.</param>
        /// <param name="emailSender">Le service d'envoi de mail à utiliser.</param>
        /// <param name="config">La configuration contenant les variables liées à la vérification des adresses mail.</param>
        public EmailVerificator(IUnitOfWorkUserServices uow, IEmailSender emailSender, IConfiguration config)
        {
            _uow = uow;
            _emailSender = emailSender;
            _config = config;
        }

        public async Task SendVerificationAsync(Utilisateur user)
        {
            if (user.Anonyme) return;

            var emailverif = await _uow.EmailVerifs.GetById(user.Id);

            bool add = emailverif is null;
            if (add) emailverif = new() { IdUtilisateur = user.Id };

            emailverif.Mail = user.Mail;
            emailverif.Code = GenerateCode();
            emailverif.Date = DateTime.Now;

            if (add) await _uow.EmailVerifs.Add(emailverif);
            else await _uow.EmailVerifs.Update(emailverif);

            user.DateVerificationEmail = null;
            await _uow.SaveChanges();

            await _emailSender.SendAsync(
                to: user.Mail,
                subject: "FIFA : Email verification",
                message: GenerateMessage(user, emailverif)
            );
        }

        public async Task<bool> VerifyAsync(Utilisateur user, string code)
        {
            if (user.Anonyme) return false;

            int expireMinutes = int.Parse(_config["EmailVerification:ExpireMinutes"]);
            var emailverif = await _uow.EmailVerifs.GetById(user.Id);

            if (emailverif is null || emailverif.Code != code) return false;

            await _uow.EmailVerifs.Delete(emailverif);
            await _uow.SaveChanges();

            if (expireMinutes > 0 && emailverif.Date.AddMinutes(expireMinutes) < DateTime.Now) return false;

            user.DateVerificationEmail = DateTime.Now;
            await _uow.SaveChanges();

            return true;
        }

        private string GenerateCode()
        {
            return Guid.NewGuid().ToString();
        }

        private string GenerateMessage(Utilisateur user, AuthEmailVerif emailVerif)
        {
            return $"<p>To access all features of the FIFA website, verify your email by clicking the link below :</p>" +
                $"<a href='{_config["EmailVerification:Url"]}?code={emailVerif.Code}'>Verify your email</a>";
        }
    }
}
