using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class VotesController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public VotesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Votes
        [HttpGet]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<VoteUtilisateur>>> GetVoteUtilisateurs()
        {
            return await _context.VoteUtilisateurs.ToListAsync();
        }

        // GET: api/Votes/5
        [HttpGet("{idtheme}/{iduser}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<VoteUtilisateur>> GetVoteUtilisateur(int idtheme, int iduser)
        {
            var voteUtilisateur = await _context.VoteUtilisateurs.FindAsync(iduser, idtheme);

            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            return voteUtilisateur;
        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idtheme}/{iduser}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutVoteUtilisateur(int idtheme, int iduser, VoteUtilisateur voteUtilisateur)
        {
            if (iduser != voteUtilisateur.IdUtilisateur || idtheme != voteUtilisateur.IdTheme)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(voteUtilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteUtilisateurExists(idtheme, iduser))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<VoteUtilisateur>> PostVoteUtilisateur(VoteUtilisateur voteUtilisateur)
        {
            await _context.VoteUtilisateurs.AddAsync(voteUtilisateur);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoteUtilisateurExists(voteUtilisateur.IdTheme, voteUtilisateur.IdUtilisateur))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVoteUtilisateur", new { idtheme = voteUtilisateur.IdTheme, iduser = voteUtilisateur.IdUtilisateur }, voteUtilisateur);
        }

        // DELETE: api/Votes/5
        [HttpDelete("{idtheme}/{iduser}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteVoteUtilisateur(int idtheme, int iduser)
        {
            var voteUtilisateur = await _context.VoteUtilisateurs.FindAsync(iduser, idtheme);
            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            _context.VoteUtilisateurs.Remove(voteUtilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoteUtilisateurExists(int idtheme, int iduser)
        {
            return (_context.VoteUtilisateurs?.Any(e => e.IdUtilisateur == iduser && e.IdTheme == idtheme)).GetValueOrDefault();
        }
    }
}
