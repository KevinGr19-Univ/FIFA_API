using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Contracts.Repository;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Models;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeVotesController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly IThemeVoteManager _manager;

        public ThemeVotesController(IThemeVoteManager manager)
        {
            _manager = manager;
        }

        // GET: api/TailleProduits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ThemeVote>>> GetThemeVotes()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ThemeVote>> GetThemeVoteById(int id)
        {
            var themeVote = await _manager.GetByIdAsync(id);
            if (themeVote is null) return NotFound();

            return themeVote;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutThemeVote(int id, ThemeVote themeVote)
        {
            if (id != themeVote.Id)
            {
                return BadRequest();
            }

            var themeVoteToUpdate = await _manager.GetByIdAsync(id);
            if (themeVoteToUpdate is null) return NotFound();

            await _manager.UpdateAsync(themeVoteToUpdate, themeVote);
            return NoContent();
        }

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<ThemeVote>> PostThemeVote(ThemeVote themeVote)
        {
            await _manager.AddAsync(themeVote);
            return CreatedAtAction("GetThemeVoteById", new { themeVote.Id }, themeVote);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteThemeVote(int id)
        {
            var themeVoteToDelete = await _manager.GetByIdAsync(id);
            if (themeVoteToDelete is null) return NotFound();

            await _manager.DeleteAsync(themeVoteToDelete);
            return NoContent();
        }
    }
}
