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
    public partial class UtilisateursController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IManagerUtilisateur _manager;

        public UtilisateursController(IManagerUtilisateur manager)
        {
            _manager = manager;
        }

        // GET: api/Utilisateurs
        /// <summary>
        /// Retourne la liste des utilisateurs.
        /// </summary>
        /// <returns>La liste des utilisateurs.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Utilisateurs/5
        /// <summary>
        /// Retourne le utilisateur avec l'id passé.
        /// </summary>
        /// <param name="id">L'id du utilisateur.</param>
        /// <returns>Le utilisateur recherché.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le utilisateur recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = await _manager.GetById(id);

            if (utilisateur is null)
            {
                return NotFound();
            }

            return Ok(utilisateur);
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un utilisateur avec les informations passés.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="id">L'id du utilisateur à modifier.</param>
        /// <param name="utilisateur">Les nouvelles informations du utilisateur.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le utilisateur recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du utilisateur sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != utilisateur.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(utilisateur);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un utilisateur.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="utilisateur">Le utilisateur à ajotuer.</param>
        /// <returns>Le utilisateur ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le utilisateur est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _manager.Add(utilisateur);
            await _manager.Save();

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        /// <summary>
        /// Supprime un utilisateur.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="id">L'id du utilisateur à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le utilisateur recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await _manager.GetById(id);
            if (utilisateur is null)
            {
                return NotFound();
            }

            await _manager.Delete(utilisateur);
            await _manager.Save();

            return NoContent();
        }
    }
}
