using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class VotesController
    {
        [HttpGet("myvotes")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<VoteUtilisateur>>> GetMyVotes()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(user.Votes);
        }

        [HttpGet("myvotes/{idtheme}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<VoteUtilisateur>> GetMyVote(int idtheme)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return await GetVoteUtilisateur(idtheme, user.Id);
        }

        [HttpPut("myvotes")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> UpdateMyVote(int idtheme, VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return await PutVoteUtilisateur(idtheme, user.Id, vote);
        }
    }
}
