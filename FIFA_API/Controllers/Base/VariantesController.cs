using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class VariantesController : ControllerBase
    {
        private readonly FifaDbContext _context;

        public VariantesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Variantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VarianteCouleurProduit>>> GetVariantes()
        {
            IQueryable<VarianteCouleurProduit> query = _context.VarianteCouleurProduits;
            if (!await this.MatchPolicyAsync(ProduitsController.SEE_POLICY)) query = query.FilterVisibles();
            return await query.ToListAsync();
        }

        // GET: api/Variantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VarianteCouleurProduit>> GetVariante(int id)
        {
            var variante = await _context.VarianteCouleurProduits.GetByIdAsync(id);
            if (variante is null) return NotFound();
            if (!variante.Visible
                && !await this.MatchPolicyAsync(ProduitsController.SEE_POLICY))
                return NotFound();

            return variante;
        }

        // PUT: api/Variantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutVariante(int id, VarianteCouleurProduit variante)
        {
            if (id != variante.Id) return BadRequest();

            var oldVariante = await _context.VarianteCouleurProduits.FindAsync(id);
            if (oldVariante is null) return NotFound();

            if (oldVariante.IdCouleur != variante.IdCouleur || oldVariante.IdProduit != variante.IdProduit)
                return BadRequest();

            await _context.UpdateEntity(variante);
            return NoContent();
        }

        // POST: api/Variantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)] // Adding color = Editing product
        public async Task<ActionResult<VarianteCouleurProduit>> PostVariante(VarianteCouleurProduit variante)
        {
            try
            {
                await _context.VarianteCouleurProduits.AddAsync(variante);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                bool combinationExists = await _context.VarianteCouleurProduits.AnyAsync(v => v.IdCouleur == variante.IdCouleur && v.IdProduit == variante.IdProduit);
                if (combinationExists) return Conflict();
                throw;
            }

            return CreatedAtAction("GetVariante", new { id = variante.Id }, variante);
        }

        // DELETE: api/Variantes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteVariante(int id)
        {
            var variante = await _context.VarianteCouleurProduits.FindAsync(id);
            if (variante is null) return NotFound();

            bool stocksExists = await _context.StockProduits.AnyAsync(s => s.IdVCProduit == id);
            if (stocksExists) return Forbid();

            _context.VarianteCouleurProduits.Remove(variante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
