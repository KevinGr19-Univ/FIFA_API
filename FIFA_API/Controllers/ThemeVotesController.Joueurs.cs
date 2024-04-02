using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class ThemeVotesController
    {
        [HttpPost("{id}/joueurs/{idjoueur}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> AddJoueurToTheme(int id, int idjoueur)
        {
            bool okTheme = await _context.ThemeVotes.AnyAsync(j => j.Id == id);
            if (!okTheme) return NotFound();

            bool okJoueur = await _context.Joueurs.AnyAsync(j => j.Id == idjoueur);
            if(!okJoueur) return NotFound();

            var themevotejoueur = new ThemeVoteJoueur()
            {
                IdJoueur = idjoueur,
                IdTheme = id
            };

            await _context.ThemeVoteJoueurs.AddAsync(themevotejoueur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/joueurs/{idjoueur}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteJoueurFromTheme(int id, int idjoueur)
        {
            var themevotejoueur = await _context.ThemeVoteJoueurs.FindAsync(id, idjoueur);

            _context.ThemeVoteJoueurs.Remove(themevotejoueur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/joueurs")]
        public async Task<ActionResult<IEnumerable<Joueur>>> GetThemeJoueurs(int id)
        {
            var theme = await _context.ThemeVotes.GetByIdAsync(id);
            if(theme is null) return NotFound();

            return Ok(theme.Joueurs);
        }
    }
}
