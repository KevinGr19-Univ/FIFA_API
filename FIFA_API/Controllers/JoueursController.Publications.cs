using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class JoueursController
    {
        /// <summary>
        /// Retourne la liste des publications mentionnant le joueur.
        /// </summary>
        /// <param name="id">L'id du joueur.</param>
        /// <returns>Un dictionnaire des publications classés par type de publication (album, article, blog, document).</returns>
        /// <response code="404">Le joueur recherché n'existe pas.</response>
        [HttpGet("{id}/publications")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Dictionary<string, List<int>>>> GetJoueurPublications(int id)
        {
            var joueur = await _context.Joueurs.GetByIdWithPublications(id);
            if (joueur is null) return NotFound();

            return Publication.SortPublicationsIds(joueur.Publications);
        }

        /// <summary>
        /// Associe une publication à un joueur.
        /// </summary>
        /// <param name="id">L'id du joueur.</param>
        /// <param name="idpub">L'id de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché ou la publication recherchée n'existe pas.</response>
        /// <response code="204">Le joueur et la publication ont été associés ou étaient déjà associés.</response>
        [HttpPost("{id}/publications/{idpub}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> AddPublicationToJoueur(int id, int idpub)
        {
            var joueur = await _context.Joueurs.GetByIdWithPublications(id);
            if (joueur is null) return NotFound();

            var pub = await _context.Publications.FindAsync(idpub);
            if (pub is null) return NotFound();

            joueur.Publications.Add(pub);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Supprime l'association entre un joueur et une publication.
        /// </summary>
        /// <param name="id">L'id du joueur.</param>
        /// <param name="idpub">L'id de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché n'existe pas ou le joueur n'est pas associé à la publication recherchée.</response>
        [HttpDelete("{id}/publications/{idpub}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> RemovePublicationToJoueur(int id, int idpub)
        {
            var joueur = await _context.Joueurs.GetByIdWithPublications(id);
            if (joueur is null) return NotFound();

            var pub = joueur.Publications.FirstOrDefault(p => p.Id == idpub);
            if (pub is null) return NotFound();

            joueur.Publications.Remove(pub);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
