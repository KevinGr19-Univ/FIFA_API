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
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            return await _dataRepository.GetAllAsync();
        }

        // GET: api/Commandes/5
        [HttpGet("{id}")]
        [ActionName("GetUtilisateurById")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
