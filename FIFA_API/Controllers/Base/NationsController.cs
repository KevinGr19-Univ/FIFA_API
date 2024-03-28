﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class NationsController : ControllerBase
    {
        public const string MANAGER_POLICY = ProduitsController.MANAGER_POLICY;

        private readonly FifaDbContext _context;

        public NationsController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Nations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nation>>> GetNations()
        {
            return await _context.Nations.ToListAsync();
        }

        // GET: api/Nations/5
        [HttpGet("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Nation>> GetNation(int id)
        {
            var nation = await _context.Nations.FindAsync(id);

            if (nation == null)
            {
                return NotFound();
            }

            return nation;
        }

        // PUT: api/Nations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutNation(int id, Nation nation)
        {
            if (id != nation.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(nation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationExists(id))
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

        // POST: api/Nations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Nation>> PostNation(Nation nation)
        {
            await _context.Nations.AddAsync(nation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNation", new { id = nation.Id }, nation);
        }

        // DELETE: api/Nations/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteNation(int id)
        {
            var nation = await _context.Nations.FindAsync(id);
            if (nation == null)
            {
                return NotFound();
            }

            _context.Nations.Remove(nation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NationExists(int id)
        {
            return (_context.Nations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}