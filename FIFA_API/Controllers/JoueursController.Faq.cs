using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FIFA_API.Controllers
{
    public partial class JoueursController
    {
        /// <summary>
        /// Retourne une question-réponse de joueur.
        /// </summary>
        /// <param name="id">L'id de la question-réponse.</param>
        /// <returns>La question-réponse recherchée.</returns>
        /// <response code="404">La question-réponse recherchée n'existe pas.</response>
        [HttpGet("faq/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ActionName("GetFaq")]
        public async Task<ActionResult<FaqJoueur>> GetFaq([FromRoute] int id)
        {
            var faq = await _context.FaqJoueurs.FindAsync(id);
            return faq is null ? NotFound() : Ok(faq);
        }

        /// <summary>
        /// Ajoute une question-réponse.
        /// </summary>
        /// <param name="faq">La question-réponse à ajouter.</param>
        /// <returns>La nouvelle question-réponse.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La question-réponse est invalide.</response>
        /// <response code="404">Le joueur de la question-réponse n'existe pas.</response>
        [HttpPost("faq")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<FaqJoueur>> PostFaq([FromBody] FaqJoueur faq)
        {
            var joueur = await _context.Joueurs.GetByIdAsync(faq.IdJoueur);
            if (joueur is null) return NotFound();

            joueur.FaqJoueurs.Add(faq);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaq", new { id = faq.Id }, faq);
        }

        /// <summary>
        /// Modifie une question-réponse.
        /// </summary>
        /// <param name="id">L'id de la question-réponse.</param>
        /// <param name="faq">Les nouvelles informations de la question-réponse à modifier.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Les nouvelles informations de la question-réponse sont invalides.</response>
        /// <response code="404">La question-réponse n'existe pas.</response>
        [HttpPut("faq/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutFaq([FromRoute] int id, [FromBody] FaqJoueur faq)
        {
            if (id != faq.Id) return BadRequest();

            try
            {
                await _context.UpdateEntity(faq);
            }
            catch (DBConcurrencyException)
            {
                if (!await _context.FaqJoueurs.AnyAsync(f => f.Id == faq.Id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Supprime une question-réponse.
        /// </summary>
        /// <param name="id">L'id de la question-réponse.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La question-réponse n'existe pas.</response>
        [HttpDelete("faq/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteFaq([FromRoute] int id)
        {
            var faq = await _context.FaqJoueurs.FindAsync(id);
            if (faq is null) return NotFound();

            _context.FaqJoueurs.Remove(faq);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
