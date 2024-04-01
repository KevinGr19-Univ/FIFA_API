using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Services
{
    public class EmailVerificator : IEmailVerificator
    {
        private readonly FifaDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public EmailVerificator(FifaDbContext context, IEmailSender emailSender, IConfiguration config)
        {
            _context = context;
            _emailSender = emailSender;
            _config = config;
        }

        public async Task SendVerificationAsync(Utilisateur user)
        {
            if (user.Anonyme) return;

            var emailverif = await _context.EmailVerifs.FindAsync(user.Id);

            bool add = emailverif is null;
            if (add) emailverif = new() { IdUtilisateur = user.Id };

            emailverif.Mail = user.Mail;
            emailverif.Code = GenerateCode();
            emailverif.Date = DateTime.Now;

            if (add) await _context.EmailVerifs.AddAsync(emailverif);
            else _context.EmailVerifs.Update(emailverif);

            user.DateVerificationEmail = null;
            await _context.SaveChangesAsync();

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
            var emailverif = await _context.EmailVerifs.FindAsync(user.Id);

            if (emailverif is null || emailverif.Code != code) return false;

            _context.EmailVerifs.Remove(emailverif);
            await _context.SaveChangesAsync();

            if (expireMinutes > 0 && emailverif.Date.AddMinutes(expireMinutes) < DateTime.Now) return false;

            user.DateVerificationEmail = DateTime.Now;
            await _context.SaveChangesAsync();

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
