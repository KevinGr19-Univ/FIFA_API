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
    public class LanguesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IManagerLangue _manager;

        public LanguesController(IManagerLangue manager)
        {
            _manager = manager;
        }

        // GET: api/Langues
        /// <summary>
        /// Retourne la liste des langues.
        /// </summary>
        /// <returns>La liste des langues.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Langue>>> GetLangues()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Langues/5
        /// <summary>
        /// Retourne une langue.
        /// </summary>
        /// <param name="id">L'id de la langue recherchée.</param>
        /// <returns>La langue recherchée.</returns>
        /// <response code="404">La langue recherchée n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Langue>> GetLangue(int id)
        {
            var langue = await _manager.GetById(id);
            return langue is null ? NotFound() : langue;
        }

        // PUT: api/Langues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une langue.
        /// </summary>
        /// <param name="id">L'id de la langue à modifier.</param>
        /// <param name="langue">Les nouvelles informations de la langue.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La langue recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la langue sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutLangue(int id, Langue langue)
        {
            if (id != langue.Id)
            {
                return BadRequest();
            }

            try
            {
                await _manager.Update(langue);
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

        // POST: api/Langues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une langue.
        /// </summary>
        /// <param name="langue">Le langue à ajouter.</param>
        /// <returns>Le nouvelle langue.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le nouvelle langue est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Langue>> PostLangue(Langue langue)
        {
            await _manager.Add(langue);
            await _manager.Save();
            return CreatedAtAction("GetLangue", new { id = langue.Id }, langue);
        }

        // DELETE: api/Langues/5
        /// <summary>
        /// Supprime une langue.
        /// </summary>
        /// <param name="id">L'id de la langue à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La langue recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteLangue(int id)
        {
            var langue = await _manager.GetById(id);
            if (langue is null)
            {
                return NotFound();
            }

            await _manager.Delete(langue);
            await _manager.Save();

            return NoContent();
        }
    }
}
