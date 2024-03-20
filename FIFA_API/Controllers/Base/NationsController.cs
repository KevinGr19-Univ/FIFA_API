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
    public class NationsController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly INationManager _manager;

        public NationsController(INationManager manager)
        {
            _manager = manager;
        }

        // GET: api/TailleProduits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Nation>>> GetNations()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Nation>> GetNationById(int id)
        {
            var nation = await _manager.GetByIdAsync(id);
            if (nation is null) return NotFound();

            return nation;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutNation(int id, Nation nation)
        {
            if (id != nation.Id)
            {
                return BadRequest();
            }

            var nationToUpdate = await _manager.GetByIdAsync(id);
            if (nationToUpdate is null) return NotFound();

            await _manager.UpdateAsync(nationToUpdate, nation);
            return NoContent();
        }

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Nation>> PostNation(Nation nation)
        {
            await _manager.AddAsync(nation);
            return CreatedAtAction("GetNationById", new { nation.Id }, nation);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteNation(int id)
        {
            var nationToDelete = await _manager.GetByIdAsync(id);
            if (nationToDelete is null) return NotFound();

            await _manager.DeleteAsync(nationToDelete);
            return NoContent();
        }
    }
}
