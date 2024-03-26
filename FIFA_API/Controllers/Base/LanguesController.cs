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
    public class LanguesController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public LanguesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Langues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Langue>>> GetLangues()
        {
            return await _context.Langues.ToListAsync();
        }

        // GET: api/Langues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Langue>> GetLangue(int id)
        {
            var langue = await _context.Langues.FindAsync(id);
            return langue is null ? NotFound() : langue;
        }

        // PUT: api/Langues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutLangue(int id, Langue langue)
        {
            if (id != langue.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(langue);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LangueExists(id))
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

        // POST: api/Langues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Langue>> PostLangue(Langue langue)
        {
            await _context.Langues.AddAsync(langue);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLangue", new { id = langue.Id }, langue);
        }

        // DELETE: api/Langues/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteLangue(int id)
        {
            var langue = await _context.Langues.FindAsync(id);
            if (langue is null)
            {
                return NotFound();
            }

            _context.Langues.Remove(langue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LangueExists(int id)
        {
            return (_context.Langues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
