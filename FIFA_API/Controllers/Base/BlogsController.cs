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
    public partial class BlogsController : ControllerBase
    {
        private readonly IUnitOfWorkPublication _uow;

        public BlogsController(IUnitOfWorkPublication uow)
        {
            _uow = uow;
        }

        // GET BY ID
        /// <summary>
        /// Retourne une publication de type blog.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type blog recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type blog ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(PublicationsController.MANAGER_POLICY);
            var blog = await _uow.Blogs.GetByIdWithAll(id, !seeAll);

            return blog is null ? NotFound() : Ok(blog);
        }

        // POST
        /// <summary>
        /// Ajoute une publication de type blog.
        /// </summary>
        /// <param name="blog">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type blog.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type blog est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            await _uow.Blogs.Add(blog);
            await _uow.SaveChanges();
            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // PUT
        /// <summary>
        /// Modifie une publication de type blog.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="blog">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type blog ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.Id) return BadRequest();

            try
            {
                await _uow.Blogs.Update(blog);
                await _uow.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!await _uow.Blogs.Exists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }
    }
}
