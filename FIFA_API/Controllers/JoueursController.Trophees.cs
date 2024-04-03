using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class JoueursController
    {
        [HttpPost("{idjoueur}/trophee/{idtrophee}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PostJoueurTrophee([FromRoute] int idjoueur, [FromRoute] int idtrophee)
        {
            var joueur = await _context.Joueurs.FindAsync(idjoueur);
            if (joueur is null) return NotFound();

            var trophee = await _context.Trophees.FindAsync(idtrophee);
            if (trophee is null) return NotFound();

            if (!joueur.Trophees.Contains(trophee))
            {
                joueur.Trophees.Add(trophee);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{idjoueur}/trophee/{idtrophee}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteJoueurTrophee([FromRoute] int idjoueur, [FromRoute] int idtrophee)
        {
            var joueur = await _context.Joueurs.GetByIdAsync(idjoueur);
            if (joueur is null) return NotFound();

            var trophee = joueur.Trophees.FirstOrDefault(t => t.Id == idjoueur);
            if (trophee is null) return NotFound();

            if (!joueur.Trophees.Remove(trophee)) return Conflict();
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
