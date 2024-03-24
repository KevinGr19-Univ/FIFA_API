using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CommandesController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly ICommandeManager _commandeManager;

        public CommandesController(ICommandeManager commandeManager)
        {
            _commandeManager = commandeManager;
        }

        // GET: api/Commandes
        /// <summary>
        /// Récupérer toutes les commandes.
        /// </summary>
        /// <returns>Http response</returns>
        /// <response code="200">Les commandes ont été récupéré.</response>
        [HttpGet]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            var commandes = await _commandeManager.GetAllAsync();
            return Ok(commandes);
        }

        // GET: api/Commandes/5
        /// <summary>
        /// Récupérer une commande grâce à son id.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id de la commande que vous voulez récupérer.</param>
        /// <response code="200">La commande a été récupéré.</response>
        /// <response code="404">La commande n'a pas été trouvé.</response>
        [HttpGet("{id}")]
        [ActionName("GetCommandeById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Commande>> GetCommandeById(int id)
        {
            var commande = await _commandeManager.GetByIdAsync(id);
            if (commande is null)
            {
                return NotFound();
            }

            return commande;
        }

        // PUT: api/Commandes/5
        /// <summary>
        /// Modifier une commande.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id de la commande que vous voulez modifier.</param>
        /// <param name="commande">La nouvelle commande.</param>
        /// <response code="204">La commande a été modifié.</response>
        /// <response code="404">La commande n'a pas été trouvé.</response>
        /// <response code="400">La nouvelle commande n'a pas le même id que l'ancienne.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (id != commande.Id)
            {
                return BadRequest();
            }

            var commandeToUpdate = await _commandeManager.GetByIdAsync(id);
            if (commandeToUpdate is null)
            {
                return NotFound();
            }
            else
            {
                await _commandeManager.UpdateAsync(commande);
                return NoContent();
            }
        }

        // POST: api/Commandes
        /// <summary>
        /// Insérer une commande.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="commande">La commande à insérer.</param>
        /// <response code="201">La commande a été inséré.</response>
        /// <response code="400">La commande n'a pas les paramètres nécessaires pour être inséré.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _commandeManager.AddAsync(commande);
            return CreatedAtAction(nameof(GetCommandeById), new { id = commande.Id }, commande);
        }

        // DELETE: api/Commandes/5
        /// <summary>
        /// Supprimer une commande grâce à son id.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id de la commande que vous voulez supprimer.</param>
        /// <response code="204">La commande a été supprimé.</response>
        /// <response code="404">La commande n'a pas été trouvé.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var commande = await _commandeManager.GetByIdAsync(id);
            if (commande is null)
            {
                return NotFound();
            }
            await _commandeManager.DeleteAsync(commande);
            return NoContent();
        }

        //private bool CommandeExists(int id)
        //{
        //    return (_context.Commandes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
