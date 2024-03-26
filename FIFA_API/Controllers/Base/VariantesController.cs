using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;

namespace FIFA_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class VariantesController : ControllerBase
    {
        public const string MANAGER_POLICY = ProduitsController.MANAGER_POLICY;

        private readonly FifaDbContext _context;

        public VariantesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Variantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VarianteCouleurProduit>>> GetVarianteCouleurProduits()
        {
            return await _context.VarianteCouleurProduits.ToListAsync();
        }

        // GET: api/Variantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VarianteCouleurProduit>> GetVarianteCouleurProduit(int id)
        {
            var varianteCouleurProduit = await _context.VarianteCouleurProduits.GetByIdAsync(id);

            if (varianteCouleurProduit == null)
            {
                return NotFound();
            }

            return varianteCouleurProduit;
        }

        // PUT: api/Variantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVarianteCouleurProduit(int id, VarianteCouleurProduit varianteCouleurProduit)
        {
            if (id != varianteCouleurProduit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(varianteCouleurProduit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VarianteCouleurProduitExists(id))
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

        // POST: api/Variantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VarianteCouleurProduit>> PostVarianteCouleurProduit(VarianteCouleurProduit varianteCouleurProduit)
        {
            await _context.VarianteCouleurProduits.AddAsync(varianteCouleurProduit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVarianteCouleurProduit", new { id = varianteCouleurProduit.Id }, varianteCouleurProduit);
        }

        // DELETE: api/Variantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVarianteCouleurProduit(int id)
        {
            var varianteCouleurProduit = await _context.VarianteCouleurProduits.FindAsync(id);
            if (varianteCouleurProduit == null)
            {
                return NotFound();
            }

            _context.VarianteCouleurProduits.Remove(varianteCouleurProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VarianteCouleurProduitExists(int id)
        {
            return (_context.VarianteCouleurProduits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
