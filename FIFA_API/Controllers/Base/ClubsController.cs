using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = JoueursController.MANAGER_POLICY;

        private readonly IManagerClub _manager;

        public ClubsController(IManagerClub manager)
        {
            _manager = manager;
        }

        // GET: api/Clubs
        /// <summary>
        /// Retourne la liste des clubs.
        /// </summary>
        /// <returns>La liste des clubs.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Clubs/5
        /// <summary>
        /// Retourne le club avec l'id passé.
        /// </summary>
        /// <param name="id">L'id du club.</param>
        /// <returns>Le club recherché.</returns>
        /// <response code="404">Le club recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Club>> GetClub(int id)
        {
            var club = await _manager.GetById(id);

            if (club == null)
            {
                return NotFound();
            }

            return Ok(club);
        }

        // PUT: api/Clubs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un club avec les informations passés.
        /// </summary>
        /// <param name="id">L'id du club à modifier.</param>
        /// <param name="club">Les nouvelles informations du club.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le club recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du club sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutClub(int id, Club club)
        {
            if (id != club.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(club);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un club.
        /// </summary>
        /// <param name="club">Le club à ajotuer.</param>
        /// <returns>Le club ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le club est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
            await _manager.Add(club);
            await _manager.Save();

            return CreatedAtAction("GetClub", new { id = club.Id }, club);
        }

        // DELETE: api/Clubs/5
        /// <summary>
        /// Supprime un club.
        /// </summary>
        /// <param name="id">L'id du club à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le club recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _manager.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            await _manager.Delete(club);
            await _manager.Save();

            return NoContent();
        }
    }
}
