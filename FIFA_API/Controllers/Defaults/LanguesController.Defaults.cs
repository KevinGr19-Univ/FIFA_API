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
    public partial class LanguesController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly ILangueManager _manager;

        public LanguesController(ILangueManager manager)
        {
            _manager = manager;
        }

        // GET: api/langue
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Langue>>> GetLangues()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/langue/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Langue>> GetLangueById(int id)
        {
            var langue = await _manager.GetByIdAsync(id);
            if (langue is null) return NotFound();

            return langue;
        }

        // PUT: api/langue/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutLangue(int id, Langue langue)
        {
            if (id != langue.Id) return BadRequest();

            var langueToUpdate = await _manager.GetByIdAsync(id);
            if (langueToUpdate is null) return NotFound();

            await _manager.UpdateAsync(langueToUpdate, langue);
            return NoContent();
        }

        // POST: api/langue
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Langue>> PostLangue(Langue langue)
        {
            await _manager.AddAsync(langue);
            return CreatedAtAction("GetLangueById", new { id = langue.Id }, langue);
        }

        // DELETE: api/langue/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteLangue(int id)
        {
            var langue = await _manager.GetByIdAsync(id);
            if (langue is null) return NotFound();

            await _manager.DeleteAsync(langue);
            return NoContent();
        }
    }
}
