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
    public partial class CouleursController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly ICouleurManager _manager;

        public CouleursController(ICouleurManager manager)
        {
            _manager = manager;
        }

        // GET: api/Couleurs
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Couleur>>> GetCouleurs()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/Couleurs/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Couleur>> GetCouleurById(int id)
        {
            var couleur = await _manager.GetByIdAsync(id);
            if (couleur is null) return NotFound();

            return couleur;
        }

        // PUT: api/Couleurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCouleur(int id, Couleur couleur)
        {
            if (id != couleur.Id)
            {
                return BadRequest();
            }

            var couleurToUpdate = await _manager.GetByIdAsync(id);
            if(couleurToUpdate is null) return NotFound();

            await _manager.UpdateAsync(couleurToUpdate, couleur);
            return NoContent();
        }

        // POST: api/Couleurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Couleur>> PostCouleur(Couleur couleur)
        {
            await _manager.AddAsync(couleur);
            return CreatedAtAction("GetCouleurById", new { couleur.Id }, couleur);
        }

        // DELETE: api/Couleurs/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCouleur(int id)
        {
            var couleur = await _manager.GetByIdAsync(id);
            if (couleur is null) return NotFound();

            await _manager.DeleteAsync(couleur);
            return NoContent();
        }
    }
}
