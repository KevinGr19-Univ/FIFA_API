using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using FIFA_API.Utils;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ThemeVotesController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public ThemeVotesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/ThemeVotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThemeVote>>> GetThemeVotes()
        {
            return await _context.ThemeVotes.ToListAsync();
        }

        // GET: api/ThemeVotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThemeVote>> GetThemeVote(int id)
        {
            var themeVote = await _context.ThemeVotes.GetByIdAsync(id);

            if (themeVote == null)
            {
                return NotFound();
            }

            return themeVote;
        }

        // PUT: api/ThemeVotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThemeVote(int id, ThemeVote themeVote)
        {
            if (id != themeVote.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(themeVote);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeVoteExists(id))
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

        // POST: api/ThemeVotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThemeVote>> PostThemeVote(ThemeVote themeVote)
        {
            await _context.ThemeVotes.AddAsync(themeVote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThemeVote", new { id = themeVote.Id }, themeVote);
        }

        // DELETE: api/ThemeVotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThemeVote(int id)
        {
            var themeVote = await _context.ThemeVotes.FindAsync(id);
            if (themeVote == null)
            {
                return NotFound();
            }

            _context.ThemeVotes.Remove(themeVote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThemeVoteExists(int id)
        {
            return (_context.ThemeVotes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
