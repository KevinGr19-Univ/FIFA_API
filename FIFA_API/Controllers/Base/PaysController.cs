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
    public class PaysController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public PaysController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Pays
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPays()
        {
            return await _context.Pays.ToListAsync();
        }

        // GET: api/Pays/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Pays>> GetPays(int id)
        {
            var pays = await _context.Pays.FindAsync(id);

            if (pays is null)
            {
                return NotFound();
            }

            return pays;
        }

        // PUT: api/Pays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(pays);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaysExists(id))
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

        // POST: api/Pays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Pays>> PostPays(Pays pays)
        {
            await _context.Pays.AddAsync(pays);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPays", new { id = pays.Id }, pays);
        }

        // DELETE: api/Pays/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePays(int id)
        {
            var pays = await _context.Pays.FindAsync(id);
            if (pays is null)
            {
                return NotFound();
            }

            _context.Pays.Remove(pays);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaysExists(int id)
        {
            return (_context.Pays?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
