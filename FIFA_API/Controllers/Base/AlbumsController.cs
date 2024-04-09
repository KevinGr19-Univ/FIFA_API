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
    public partial class AlbumsController : ControllerBase
    {
        private readonly IUnitOfWorkPublication _uow;

        public AlbumsController(IUnitOfWorkPublication uow)
        {
            _uow = uow;
        }

        // GET BY ID
        /// <summary>
        /// Retourne une publication de type album.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type album recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type album ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Album>> GetAlbum(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(PublicationsController.MANAGER_POLICY);
            var album = await _uow.Albums.GetByIdWithPhotos(id, !seeAll);

            return album is null ? NotFound() : Ok(album);
        }

        // POST
        /// <summary>
        /// Ajoute une publication de type album.
        /// </summary>
        /// <param name="album">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type album.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type album est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            await _uow.Albums.Add(album);
            await _uow.SaveChanges();
            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // PUT
        /// <summary>
        /// Modifie une publication de type album.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="album">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type album ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != album.Id) return BadRequest();

            if (!await _uow.Albums.Exists(id)) return NotFound();

            await _uow.Albums.Update(album);
            await _uow.SaveChanges();

            return NoContent();
        }
    }
}
