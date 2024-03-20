using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FIFA_API.Utils;
using FIFA_API.Models.Controllers;

namespace FIFA_API.Controllers
{
    public partial class UtilisateursController
    {
        [HttpGet("GetInfo")]
        [ActionName("GetInfo")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<UserInfo>> GetInfo()
        {
            var user = await this.UtilisateurAsync();
            if (user is null)
            {
                return Unauthorized();
            }
            return UserInfo.FromUser(user);
        }

        [HttpPost("UpdateInfo")]
        [ActionName("UpdateInfo")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> UpdateInfo([FromBody] UserInfo userInfo)
        {
            // TODO: A tester
            var user = await this.UtilisateurAsync();
            if (user is null)
            {
                return Unauthorized();
            }

            var newUser = userInfo.UpdateUser(user);

            newUser.Id = user.Id;
            return await PutUtilisateur(user.Id, newUser);
        }
    }
}
