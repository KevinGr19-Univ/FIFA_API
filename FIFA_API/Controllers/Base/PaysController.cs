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
    public partial class PaysController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly IPaysManager _manager;

        public PaysController(IPaysManager manager)
        {
            _manager = manager;
        }

        // GET: api/Pays
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPays()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/Pays/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Pays>> GetPays(int id)
        {
            var pays = await _manager.GetByIdAsync(id);
            if (pays is null) return NotFound();

            return pays;
        }

        // PUT: api/Pays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id) return BadRequest();

            var paysToUpdate = await _manager.GetByIdAsync(id);
            if (paysToUpdate is null) return NotFound();

            await _manager.UpdateAsync(pays);
            return NoContent();
        }

        // POST: api/Pays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Pays>> PostPays(Pays pays)
        {
            await _manager.AddAsync(pays);
            return CreatedAtAction("GetPays", new { id = pays.Id }, pays);
        }

        // DELETE: api/Pays/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePays(int id)
        {
            var pays = await _manager.GetByIdAsync(id);
            if (pays is null) return NotFound();

            await _manager.DeleteAsync(pays);
            return NoContent();
        }
    }
}
