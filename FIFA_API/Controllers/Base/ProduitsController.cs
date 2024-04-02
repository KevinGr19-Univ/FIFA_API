using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProduitsController : ControllerBase
    {
        public const string SEE_POLICY = Policies.DirecteurVente;
        public const string ADD_POLICY = Policies.DirecteurVente;
        public const string EDIT_POLICY = Policies.DirecteurVente;
        public const string DELETE_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public ProduitsController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Produits
        [HttpGet]
        [Authorize(Policy = ADD_POLICY)]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            IQueryable<Produit> query = _context.Produits;
            if (!await this.MatchPolicyAsync(SEE_POLICY)) query = query.FilterVisibles();
            return await query.ToListAsync();
        }

        // GET: api/Produits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var produit = await _context.Produits.GetByIdAsync(id);

            if (produit is null)  return NotFound();
            if (!produit.Visible
                && !await this.MatchPolicyAsync(SEE_POLICY))
                return NotFound();

            return produit;
        }

        // PUT: api/Produits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(produit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(id))
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

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = ADD_POLICY)]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            await _context.Produits.AddAsync(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduit", new { id = produit.Id }, produit);
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = DELETE_POLICY)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduitExists(int id)
        {
            return (_context.Produits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
