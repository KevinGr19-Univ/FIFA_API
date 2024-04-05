using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class JoueursController
    {
        /// <summary>
        /// Associe un joueur et un trophée.
        /// </summary>
        /// <param name="idjoueur">L'id du joueur.</param>
        /// <param name="idtrophee">L'id du trophée.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché ou le trophée recherché n'existe pas.</response>
        /// <response code="204">Le joueur et le trophée ont été associés ou étaient déjà associés.</response>
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

        /// <summary>
        /// Supprime l'association entre un joueur et un trophée.
        /// </summary>
        /// <param name="idjoueur">L'id du joueur.</param>
        /// <param name="idtrophee">L'id du trophée.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché n'existe pas ou le joueur n'est pas associé au trophée recherchée.</response>
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
