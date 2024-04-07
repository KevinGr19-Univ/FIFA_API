using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaysController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IManagerPays _manager;

        public PaysController(IManagerPays manager)
        {
            _manager = manager;
        }

        // GET: api/Pays
        /// <summary>
        /// Retourne la liste des pays.
        /// </summary>
        /// <returns>La liste des pays.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPays()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Pays/5
        /// <summary>
        /// Retourne le pays avec l'id passé.
        /// </summary>
        /// <param name="id">L'id du pays.</param>
        /// <returns>Le pays recherché.</returns>
        /// <response code="404">Le pays recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Pays>> GetPays(int id)
        {
            var pays = await _manager.GetById(id);

            if (pays is null)
            {
                return NotFound();
            }

            return pays;
        }

        // PUT: api/Pays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un pays avec les informations passés.
        /// </summary>
        /// <param name="id">L'id du pays à modifier.</param>
        /// <param name="pays">Les nouvelles informations du pays.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le pays recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du pays sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest();
            }

            try
            {
                await _manager.Update(pays);
                await _manager.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _manager.Exists(id))
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

        // POST: api/Pays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un pays.
        /// </summary>
        /// <param name="pays">Le pays à ajotuer.</param>
        /// <returns>Le pays ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le pays est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Pays>> PostPays(Pays pays)
        {
            await _manager.Add(pays);
            await _manager.Save();

            return CreatedAtAction("GetPays", new { id = pays.Id }, pays);
        }

        // DELETE: api/Pays/5
        /// <summary>
        /// Supprime un pays.
        /// </summary>
        /// <param name="id">L'id du pays à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le pays recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePays(int id)
        {
            var pays = await _manager.GetById(id);
            if (pays is null)
            {
                return NotFound();
            }

            await _manager.Delete(pays);
            await _manager.Save();

            return NoContent();
        }
    }
}
