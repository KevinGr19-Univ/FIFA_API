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
        [HttpGet("{id}/publications")]
        public async Task<ActionResult<Dictionary<string, List<int>>>> GetJoueurPublications(int id)
        {
            var joueur = await _context.Joueurs.GetByIdWithPublications(id);
            if (joueur is null) return NotFound();

            return Publication.SortPublicationsIds(joueur.Publications);
        }

        [HttpPost("{id}/publications/{idpub}")]
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

        [HttpDelete("{id}/publications/{idpub}")]
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
