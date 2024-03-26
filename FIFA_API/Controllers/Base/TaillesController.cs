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
    [Route("api/tailles")]
    [ApiController]
    public partial class TaillesController : ControllerBase
    {
        public const string MANAGER_POLICY = ProduitsController.MANAGER_POLICY;

        private readonly FifaDbContext _context;

        public TaillesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/TailleProduits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TailleProduit>>> GetTailleProduits()
        {
            return await _context.TailleProduits.ToListAsync();
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TailleProduit>> GetTailleProduit(int id)
        {
            var tailleProduit = await _context.TailleProduits.FindAsync(id);

            if (tailleProduit == null)
            {
                return NotFound();
            }

            return tailleProduit;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTailleProduit(int id, TailleProduit tailleProduit)
        {
            if (id != tailleProduit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(tailleProduit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TailleProduitExists(id))
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

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<TailleProduit>> PostTailleProduit(TailleProduit tailleProduit)
        {
            await _context.TailleProduits.AddAsync(tailleProduit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTailleProduit", new { id = tailleProduit.Id }, tailleProduit);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTailleProduit(int id)
        {
            var tailleProduit = await _context.TailleProduits.FindAsync(id);
            if (tailleProduit == null)
            {
                return NotFound();
            }

            _context.TailleProduits.Remove(tailleProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TailleProduitExists(int id)
        {
            return (_context.TailleProduits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
