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

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CategoriesController : ControllerBase
    {
        private readonly FifaDbContext _context;

        public CategoriesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorieProduit>>> GetCategorieProduits()
        {
            return await _context.CategorieProduits.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategorieProduit>> GetCategorieProduit(int id)
        {
            var categorieProduit = await _context.CategorieProduits.FindAsync(id);

            if (categorieProduit == null)
            {
                return NotFound();
            }

            return categorieProduit;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutCategorieProduit(int id, CategorieProduit categorieProduit)
        {
            if (id != categorieProduit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(categorieProduit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieProduitExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<CategorieProduit>> PostCategorieProduit(CategorieProduit categorieProduit)
        {
            await _context.CategorieProduits.AddAsync(categorieProduit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategorieProduit", new { id = categorieProduit.Id }, categorieProduit);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteCategorieProduit(int id)
        {
            var categorieProduit = await _context.CategorieProduits.FindAsync(id);
            if (categorieProduit == null)
            {
                return NotFound();
            }

            _context.CategorieProduits.Remove(categorieProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategorieProduitExists(int id)
        {
            return (_context.CategorieProduits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
