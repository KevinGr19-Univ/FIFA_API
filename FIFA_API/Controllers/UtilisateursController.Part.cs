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
        public async Task<IActionResult> UpdateInfo(UserInfo userInfo)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            userInfo.UpdateUser(user);
            await _context.UpdateEntity(user);
            return NoContent();
        }
    }
}
