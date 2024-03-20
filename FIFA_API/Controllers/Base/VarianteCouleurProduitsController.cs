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
    public class VarianteCouleurProduitsController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.DirecteurVente;

        private readonly IVarianteCouleurProduitManager _manager;

        public VarianteCouleurProduitsController(IVarianteCouleurProduitManager manager)
        {
            _manager = manager;
        }

        // GET: api/TailleProduits
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VarianteCouleurProduit>>> GetVarianteCouleurProduits()
        {
            return Ok(await _manager.GetAllAsync());
        }

        // GET: api/TailleProduits/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<VarianteCouleurProduit>> GetVarianteCouleurProduitById(int id)
        {
            var varianteCouleurProduit = await _manager.GetByIdAsync(id);
            if (varianteCouleurProduit is null) return NotFound();

            return varianteCouleurProduit;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutVarianteCouleurProduit(int id, VarianteCouleurProduit varianteCouleurProduit)
        {
            if (id != varianteCouleurProduit.Id)
            {
                return BadRequest();
            }

            var varianteCouleurProduitToUpdate = await _manager.GetByIdAsync(id);
            if (varianteCouleurProduitToUpdate is null) return NotFound();

            await _manager.UpdateAsync(varianteCouleurProduitToUpdate, varianteCouleurProduit);
            return NoContent();
        }

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<VarianteCouleurProduit>> PostVarianteCouleurProduit(VarianteCouleurProduit varianteCouleurProduit)
        {
            await _manager.AddAsync(varianteCouleurProduit);
            return CreatedAtAction("GetVarianteCouleurProduitById", new { varianteCouleurProduit.Id }, varianteCouleurProduit);
        }

        // DELETE: api/TailleProduits/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteVarianteCouleurProduit(int id)
        {
            var varianteCouleurProduitToDelete = await _manager.GetByIdAsync(id);
            if (varianteCouleurProduitToDelete is null) return NotFound();

            await _manager.DeleteAsync(varianteCouleurProduitToDelete);
            return NoContent();
        }
    }
}
