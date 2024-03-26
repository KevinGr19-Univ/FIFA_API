using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouleursController : ControllerBase
    {
        public const string MANAGER_POLICY = ProduitsController.MANAGER_POLICY;

        private readonly FifaDbContext _context;

        public CouleursController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Couleurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Couleur>>> GetCouleurs()
        {
            return await _context.Couleurs.ToListAsync();
        }

        // GET: api/Couleurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Couleur>> GetCouleur(int id)
        {
            var couleur = await _context.Couleurs.FindAsync(id);

            if (couleur == null)
            {
                return NotFound();
            }

            return couleur;
        }

        // PUT: api/Couleurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCouleur(int id, Couleur couleur)
        {
            if (id != couleur.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(couleur);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouleurExists(id))
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

        // POST: api/Couleurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Couleur>> PostCouleur(Couleur couleur)
        {
            await _context.Couleurs.AddAsync(couleur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCouleur", new { id = couleur.Id }, couleur);
        }

        // DELETE: api/Couleurs/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCouleur(int id)
        {
            var couleur = await _context.Couleurs.FindAsync(id);
            if (couleur == null)
            {
                return NotFound();
            }

            _context.Couleurs.Remove(couleur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CouleurExists(int id)
        {
            return (_context.Couleurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
