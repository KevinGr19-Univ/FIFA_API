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
        [HttpGet("faq/{id}")]
        [ActionName("GetFaq")]
        public async Task<ActionResult<FaqJoueur>> GetFaq([FromRoute] int id)
        {
            var faq = await _context.FaqJoueurs.FindAsync(id);
            return faq is null ? NotFound() : Ok(faq);
        }

        [HttpPost("faq")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<FaqJoueur>> PostFaq([FromBody] FaqJoueur faq)
        {
            var joueur = await _context.Joueurs.GetByIdAsync(faq.IdJoueur);
            if (joueur is null) return NotFound();

            joueur.FaqJoueurs.Add(faq);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaq", new { id = faq.Id }, faq);
        }

        [HttpPut("faq/{id}")]
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

        [HttpDelete("faq/{id}")]
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
