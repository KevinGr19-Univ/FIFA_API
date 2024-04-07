using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/publications/[controller]")]
    [ApiController]
    public partial class DocumentsController : ControllerBase
    {
        private readonly IUnitOfWorkPublication _uow;

        public DocumentsController(IUnitOfWorkPublication uow)
        {
            _uow = uow;
        }

        // GET BY ID
        /// <summary>
        /// Retourne une publication de type document.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type document recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type document ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(PublicationsController.MANAGER_POLICY);
            var document = await _uow.Documents.GetByIdWithAll(id, !seeAll);

            return document is null ? NotFound() : Ok(document);
        }

        // POST
        /// <summary>
        /// Ajoute une publication de type document.
        /// </summary>
        /// <param name="document">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type document.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type document est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<ActionResult<Document>> PostDocument(Document document)
        {
            await _uow.Documents.Add(document);
            await _uow.SaveChanges();
            return CreatedAtAction("GetDocument", new { id = document.Id }, document);
        }

        // PUT
        /// <summary>
        /// Modifie une publication de type document.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="document">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type document ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (id != document.Id) return BadRequest();

            try
            {
                await _uow.Documents.Update(document);
                await _uow.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!await _uow.Documents.Exists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }
    }
}
