using FIFA_API.Contracts;
using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class UtilisateursController
    {
        [HttpGet("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<UserInfo>> GetInfo()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return UserInfo.FromUser(user);
        }

        [HttpPost("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> UpdateInfo(UserInfo userInfo, [FromServices] IEmailVerificator emailVerificator)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            bool emailChanged = user.Mail != userInfo.Mail;
            userInfo.UpdateUser(user);

            await _context.UpdateEntity(user);
            if (emailChanged)
            {
                await emailVerificator.SendVerificationAsync(user);
                return Ok(new { verification_email_sent = true });
            }

            return NoContent();
        }

        [HttpDelete("me/anonymize")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> AnonymizeAccount([FromBody] AnonymizeRequest req, [FromServices] IPasswordHasher passwordHasher)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null)
                return Unauthorized();

            if (!passwordHasher.Verify(user.HashMotDePasse, req.Password))
                return Unauthorized();

            user.Anonymize();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteRequest req, [FromServices] IPasswordHasher passwordHasher)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null)
                return Unauthorized();

            if (!passwordHasher.Verify(user.HashMotDePasse, req.Password))
                return Unauthorized();

            _context.Utilisateurs.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
