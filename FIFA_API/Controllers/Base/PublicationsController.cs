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

        private readonly FifaDbContext _context;

        public PublicationsController(FifaDbContext context)
        {
            _context = context;
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
            return await _context.Publications.ToListAsync();
        }

        #region Generic actions
        /// <summary>
        /// Retourne la liste des publications de type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <returns>La liste des publications de type <typeparamref name="T"/>.</returns>
        [NonAction]
        public async Task<ActionResult<IEnumerable<T>>> GetPublications<T>() where T : Publication
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Retourne une publication de type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type <typeparamref name="T"/> recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type <typeparamref name="T"/> ou a été filtrée.</response>
        [NonAction]
        public async Task<ActionResult<T>> GetPublication<T>(int id) where T : Publication
        {
            var publication = await _context.Set<T>().GetByIdAsync(id);
            if (publication == null) return NotFound();
            if (!publication.Visible
                && !await this.MatchPolicyAsync(ProduitsController.SEE_POLICY))
                return NotFound();

            return publication;
        }

        /// <summary>
        /// Modifie une publication de type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="publication">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type <typeparamref name="T"/> ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [NonAction]
        public async Task<IActionResult> PutPublication<T>(int id, T publication) where T : Publication
        {
            if (id != publication.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(publication);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists<T>(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Ajoute une publication de type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="publication">La publication à ajouter.</param>
        /// <param name="createdAtAction">La route à utiliser pour la réponse <see cref="CreatedResult"/>.</param>
        /// <returns>La nouvelle publication de type <typeparamref name="T"/>.</returns>
        /// <response code="400">La publication de type <typeparamref name="T"/> est invalide.</response>
        [NonAction]
        public async Task<ActionResult<T>> PostPublication<T>(T publication, string createdAtAction) where T : Publication
        {
            publication.DatePublication = DateTime.Now;
            await _context.Set<T>().AddAsync(publication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(createdAtAction, new { id = publication.Id }, publication);
        }

        private bool PublicationExists<T>(int id) where T : Publication
        {
            return (_context.Publications?.Any(e => e.Id == id && e is T)).GetValueOrDefault();
        }
        #endregion

        // GET BY ID
        /// <summary>
        /// Retourne une publication de type album.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type album recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type album ou a été filtrée.</response>
        [HttpGet("albums/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Album>> GetAlbum(int id) => await GetPublication<Album>(id);

        /// <summary>
        /// Retourne une publication de type article.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type article recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type article ou a été filtrée.</response>
        [HttpGet("articles/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Article>> GetArticle(int id) => await GetPublication<Article>(id);

        /// <summary>
        /// Retourne une publication de type blog.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type blog recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type blog ou a été filtrée.</response>
        [HttpGet("blogs/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Blog>> GetBlog(int id) => await GetPublication<Blog>(id);

        /// <summary>
        /// Retourne une publication de type document.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type document recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type document ou a été filtrée.</response>
        [HttpGet("documents/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Document>> GetDocument(int id) => await GetPublication<Document>(id);

        // POST
        /// <summary>
        /// Ajoute une publication de type album.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="album">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type album.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type album est invalide.</response>
        [HttpPost("albums")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Album>> PostAlbum(Album album) => await PostPublication(album, "GetAlbum");

        /// <summary>
        /// Ajoute une publication de type article.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="article">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type article.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type article est invalide.</response>
        [HttpPost("articles")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Article>> PostArticle(Article article) => await PostPublication(article, "GetArticle");

        /// <summary>
        /// Ajoute une publication de type blog.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="blog">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type blog.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type blog est invalide.</response>
        [HttpPost("blogs")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog) => await PostPublication(blog, "GetBlog");

        /// <summary>
        /// Ajoute une publication de type document.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="document">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type document.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type document est invalide.</response>
        [HttpPost("documents")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Document>> PostDocument(Document document) => await PostPublication(document, "GetDocument");

        // PUT
        /// <summary>
        /// Modifie une publication de type album.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="album">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type album ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("albums/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutAlbum(int id, Album album) => await PutPublication(id, album);

        /// <summary>
        /// Modifie une publication de type article.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="article">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type article ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("articles/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutArticle(int id, Article article) => await PutPublication(id, article);

        /// <summary>
        /// Modifie une publication de type blog.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="blog">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type blog ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("blogs/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutBlog(int id, Blog blog) => await PutPublication(id, blog);

        /// <summary>
        /// Modifie une publication de type document.
        /// </summary>
        /// <typeparam name="T">Le type de publication.</typeparam>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="document">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type document ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("documents/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutDocument(int id, Document document) => await PutPublication(id, document);

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
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
