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
    public class CompetitionsController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly ICompetitionManager _manager;

        public CompetitionsController(ICompetitionManager manager)
        {
            _manager = manager;
        }

        // GET: api/TailleProduits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Competition>> GetCompetitionById(int id)
        {
            var comp = await _manager.GetByIdAsync(id);
            if (comp is null) return NotFound();

            return comp;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCompetition(int id, Competition competition)
        {
            if (id != competition.Id)
            {
                return BadRequest();
            }

            var compToUpdate = await _manager.GetByIdAsync(id);
            if (compToUpdate is null) return NotFound();

            await _manager.UpdateAsync(compToUpdate, competition);
            return NoContent();
        }

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            await _manager.AddAsync(competition);
            return CreatedAtAction("GetCompetitionById", new { competition.Id }, competition);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var compToDelete = await _manager.GetByIdAsync(id);
            if (compToDelete is null) return NotFound();

            await _manager.DeleteAsync(compToDelete);
            return NoContent();
        }
    }
}
