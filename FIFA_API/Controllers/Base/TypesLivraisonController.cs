using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class TypesLivraisonController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public TypesLivraisonController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/TypesLivraison
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeLivraison>>> GetTypeLivraisons()
        {
            return await _context.TypeLivraisons.ToListAsync();
        }

        // GET: api/TypesLivraison/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeLivraison>> GetTypeLivraison(int id)
        {
            var typeLivraison = await _context.TypeLivraisons.FindAsync(id);

            if (typeLivraison is null)
            {
                return NotFound();
            }

            return typeLivraison;
        }

        // PUT: api/TypesLivraison/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTypeLivraison(int id, TypeLivraison typeLivraison)
        {
            if (id != typeLivraison.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(typeLivraison);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeLivraisonExists(id))
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

        // POST: api/TypesLivraison
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<TypeLivraison>> PostTypeLivraison(TypeLivraison typeLivraison)
        {
            await _context.TypeLivraisons.AddAsync(typeLivraison);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeLivraison", new { id = typeLivraison.Id }, typeLivraison);
        }

        // DELETE: api/TypesLivraison/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTypeLivraison(int id)
        {
            var typeLivraison = await _context.TypeLivraisons.FindAsync(id);
            if (typeLivraison == null)
            {
                return NotFound();
            }

            _context.TypeLivraisons.Remove(typeLivraison);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeLivraisonExists(int id)
        {
            return (_context.TypeLivraisons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
