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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<UserInfo>> GetInfo()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return UserInfo.FromUser(user);
        }

        [HttpPost("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> UpdateInfo(
            UserInfo userInfo,
            [FromServices] IEmailVerificator emailVerificator,
            [FromServices] ILogin2FAService login2FAService)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            Dictionary<string, object> res = new();
            bool emailChanged = user.Mail != userInfo.Mail;
            bool phoneChanged = user.Telephone != userInfo.Telephone;
            bool was2fa = user.Login2FA;

            userInfo.UpdateUser(user);
            await _context.UpdateEntity(user);

            if (emailChanged)
            {
                await emailVerificator.SendVerificationAsync(user);
                res["verification_email_sent"] = true;
            }

            if (phoneChanged)
            {
                user.DateVerif2FA = null;
                await _context.SaveChangesAsync();
                await login2FAService.Remove2FACode(user);

                if (user.Telephone is not null && user.DoubleAuthentification)
                {
                    var token = await login2FAService.Send2FACodeAsync(user);
                    res["verification_2fa_token"] = token;
                }

                else if (was2fa) res["2fa_disabled"] = true;
            }

            return Ok(res);
        }

        [HttpDelete("me/anonymize")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
