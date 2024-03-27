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
    public partial class TropheesController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public TropheesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Trophees
        [HttpGet]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Trophee>>> GetTrophees()
        {
          if (_context.Trophees == null)
          {
              return NotFound();
          }
            return await _context.Trophees.ToListAsync();
        }

        // GET: api/Trophees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trophee>> GetTrophee(int id)
        {
            var trophee = await _context.Trophees.FindAsync(id);

            if (trophee == null)
            {
                return NotFound();
            }

            return trophee;
        }

        // PUT: api/Trophees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTrophee(int id, Trophee trophee)
        {
            if (id != trophee.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(trophee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TropheeExists(id))
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

        // POST: api/Trophees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Trophee>> PostTrophee(Trophee trophee)
        {
            await _context.Trophees.AddAsync(trophee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrophee", new { id = trophee.Id }, trophee);
        }

        // DELETE: api/Trophees/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTrophee(int id)
        {
            var trophee = await _context.Trophees.FindAsync(id);
            if (trophee == null)
            {
                return NotFound();
            }

            _context.Trophees.Remove(trophee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TropheeExists(int id)
        {
            return (_context.Trophees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
