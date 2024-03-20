using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TailleProduitsController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly ITailleProduitManager _manager;

        public TailleProduitsController(ITailleProduitManager manager)
        {
            _manager = manager;
        }

        // GET: api/TailleProduits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TailleProduit>>> GetTailles()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TailleProduit>> GetTailleById(int id)
        {
            var taille = await _manager.GetByIdAsync(id);
            if (taille is null) return NotFound();

            return taille;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTailleProduit(int id, TailleProduit tailleProduit)
        {
            if (id != tailleProduit.Id)
            {
                return BadRequest();
            }

            var tailleToUpdate = await _manager.GetByIdAsync(id);
            if (tailleToUpdate is null) return NotFound();

            await _manager.UpdateAsync(tailleToUpdate, tailleProduit);
            return NoContent();
        }

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<TailleProduit>> PostTailleProduit(TailleProduit tailleProduit)
        {
            await _manager.AddAsync(tailleProduit);
            return CreatedAtAction("GetTailleById", new { tailleProduit.Id }, tailleProduit);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTailleProduit(int id)
        {
            var tailleToDelete = await _manager.GetByIdAsync(id);
            if (tailleToDelete is null) return NotFound();

            await _manager.DeleteAsync(tailleToDelete);
            return NoContent();
        }
    }
}
