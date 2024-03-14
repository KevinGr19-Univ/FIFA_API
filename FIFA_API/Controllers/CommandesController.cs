using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly BaseRepository<Commande> _dataRepository;
        

        public CommandesController(BaseRepository<Commande> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // GET: api/Commandes
        /// <summary>
        /// Récupérer toutes les commandes.
        /// </summary>
        /// <returns>Http response</returns>
        /// <response code="200">Les commandes ont été récupéré.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            return await _dataRepository.GetAllAsync();
        }

        // GET: api/Commandes/5
        /// <summary>
        /// Récupérer une commande grâce à son id.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id de la commande que vous voulez modifier.</param>
        /// <response code="200">La commande a été récupéré.</response>
        /// <response code="404">La commande n'a pas été trouvé.</response>
        [HttpGet("{id}")]
        [ActionName("GetCommandeById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Commande>> GetCommandeById(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
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
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (id != commande.Id)
            {
                return BadRequest();
            }

            var commandeToUpdate = await _dataRepository.GetByIdAsync(id);
            if (commandeToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await _dataRepository.UpdateAsync(commandeToUpdate.Value, commande);
                return NoContent();
            }
        }

        // POST: api/Commandes
        /// <summary>
        /// Insérer une commande.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="commande">La commande a inséré.</param>
        /// <response code="201">La commande a été inséré.</response>
        /// <response code="400">La commande n'a pas les paramètres nécessaires pour être inséré.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dataRepository.AddAsync(commande);
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
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }

        //private bool CommandeExists(int id)
        //{
        //    return (_context.Commandes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
