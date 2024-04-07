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
    public partial class PublicationsController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IUnitOfWorkPublication _uow;

        public PublicationsController(IUnitOfWorkPublication uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Retourne la liste de toutes les publications.
        /// </summary>
        /// <returns>La liste des publications.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Publication>>> GetAllPublications()
        {
            return Ok(await _uow.Publications.GetAll());
        }

        /// <summary>
        /// Retourne une publication.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Publication>> GetPublication(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var publication = await _uow.Publications.GetByIdWithPhoto(id, !seeAll);

            if (publication == null) return NotFound();
            return publication;
        }

        /// <summary>
        /// Supprime une publication.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePublication(int id)
        {
            var publication = await _uow.Publications.GetById(id);
            if (publication == null)
            {
                return NotFound();
            }

            await _uow.Publications.Delete(publication);
            await _uow.SaveChanges();

            return NoContent();
        }
    }
}
