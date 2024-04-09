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
    public partial class TropheesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IManagerTrophee _manager;

        public TropheesController(IManagerTrophee manager)
        {
            _manager = manager;
        }

        // GET: api/Trophees
        /// <summary>
        /// Retourne la liste des trophées.
        /// </summary>
        /// <returns>La liste des trophées.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Trophee>>> GetTrophees()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Trophees/5
        /// <summary>
        /// Retourne le trophée avec l'id passé.
        /// </summary>
        /// <param name="id">L'id du trophée.</param>
        /// <returns>Le trophée recherché.</returns>
        /// <response code="404">Le trophée recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Trophee>> GetTrophee(int id)
        {
            var trophee = await _manager.GetById(id);

            if (trophee == null) return NotFound();
            return Ok(trophee);
        }

        /// <summary>
        /// Retourne la liste des joueurs ayant reçu un trophée.
        /// </summary>
        /// <param name="id">L'id du trophée recherché.</param>
        /// <returns>La liste des joueurs associés.</returns>
        /// <response code="404">Le trophée recherché n'existe pas.</response>
        [HttpGet("{id}/joueurs")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Joueur>>> GetTropheeJoueurs(int id)
        {
            var trophee = await _manager.GetByIdWithJoueurs(id);

            if (trophee == null) return NotFound();
            return Ok(trophee.Joueurs);
        }

        // PUT: api/Trophees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un trophée avec les informations passés.
        /// </summary>
        /// <param name="id">L'id du trophée à modifier.</param>
        /// <param name="trophee">Les nouvelles informations du trophée.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le trophée recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du trophée sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTrophee(int id, Trophee trophee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != trophee.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(trophee);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Trophees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un trophée.
        /// </summary>
        /// <param name="trophee">Le trophée à ajotuer.</param>
        /// <returns>Le trophée ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le trophée est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Trophee>> PostTrophee(Trophee trophee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _manager.Add(trophee);
            await _manager.Save();

            return CreatedAtAction("GetTrophee", new { id = trophee.Id }, trophee);
        }

        // DELETE: api/Trophees/5
        /// <summary>
        /// Supprime un trophée.
        /// </summary>
        /// <param name="id">L'id du trophée à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le trophée recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTrophee(int id)
        {
            var trophee = await _manager.GetById(id);
            if (trophee == null)
            {
                return NotFound();
            }

            await _manager.Delete(trophee);
            await _manager.Save();

            return NoContent();
        }
    }
}
