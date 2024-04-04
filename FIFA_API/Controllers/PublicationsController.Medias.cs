using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FIFA_API.Controllers
{
    public partial class PublicationsController
    {
        /// <summary>
        /// Ajoute une photo à une publication, en créeant la photo.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="photo">La photo à ajouter.</param>
        /// <returns>La nouvelle photo.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, ou ne supporte pas l'ajout de photo. (Acceptés: <see cref="Album"/>, <see cref="Article"/>, <see cref="Blog"/>)</response>
        [HttpPost("{id}/photo")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> PostPhoto(int id, Photo photo)
        {
            var publication = await _context.Publications.GetByIdAsync(id);
            if (publication is null) return NotFound();

            return await PostPhoto(publication, photo);
        }

        /// <summary>
        /// Associe une photo existante à une publication.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="idphoto">L'id de la photo</param>
        /// <returns>La photo associée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, ou ne supporte pas l'ajout de photo. (Acceptés: <see cref="Album"/>, <see cref="Article"/>, <see cref="Blog"/>)</response>
        [HttpPost("{id}/photo/{idphoto}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> PostPhoto(int id, int idphoto)
        {
            var publication = await _context.Publications.GetByIdAsync(id);
            if (publication is null) return NotFound();

            var photo = await _context.Photos.FindAsync(idphoto);
            if (photo is null) return NotFound();

            return await PostPhoto(publication, photo);
        }

        private async Task<ActionResult<Photo>> PostPhoto(Publication publication, Photo photo)
        {
            if (publication is Album album) album.Photos.Add(photo);
            else if (publication is Article article) article.Photos.Add(photo);
            else if (publication is Blog blog) blog.Photos.Add(photo);
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return Ok(photo);
        }

        /// <summary>
        /// Supprime l'association entre une publication et une photo.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="idphoto">L'id de la photo.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication n'existe pas ou n'est pas associée avec la photo recherchée.</response>
        [HttpDelete("{id}/photo/{idphoto}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePhoto(int id, int idphoto)
        {
            var publication = await _context.Publications.GetByIdAsync(id);
            if (publication is null) return NotFound();

            ICollection<Photo> photos;
            if (publication is Album album) photos = album.Photos;
            else if (publication is Article article) photos = article.Photos;
            else if (publication is Blog blog) photos = blog.Photos;
            else
            {
                return NotFound();
            }

            var photo = photos.FirstOrDefault(p => p.Id == idphoto);
            if (photo is null) return NotFound();

            photos.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Ajoute une vidéo à un article, en créeant la vidéo.
        /// </summary>
        /// <param name="id">L'id de l'article.</param>
        /// <param name="video">La vidéo à ajouter.</param>
        /// <returns>La nouvelle vidéo.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">L'article recherché n'existe pas.</response>
        [HttpPost("{id}/video")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Video>> PostVideo(int id, Video video)
        {
            var article = await _context.Articles.GetByIdAsync(id);
            if (article is null) return NotFound();

            return await PostVideo(article, video);
        }

        /// <summary>
        /// Associe une vidéo existante à un article.
        /// </summary>
        /// <param name="id">L'id de l'article.</param>
        /// <param name="idvideo">L'id de la vidéo.</param>
        /// <returns>La vidéo associée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">L'article recherché n'existe pas.</response>
        [HttpPost("{id}/video/{idvideo}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Video>> PostVideo(int id, int idvideo)
        {
            var article = await _context.Articles.GetByIdAsync(id);
            if (article is null) return NotFound();

            var video = await _context.Videos.FindAsync(idvideo);
            if (video is null) return NotFound();

            return await PostVideo(article, video);
        }

        private async Task<ActionResult<Video>> PostVideo(Article article, Video video)
        {
            article.Videos.Add(video);
            await _context.SaveChangesAsync();
            return Ok(video);
        }

        /// <summary>
        /// Supprime l'association entre un article et une vidéo.
        /// </summary>
        /// <param name="id">L'id de l'article.</param>
        /// <param name="idvideo">L'id de la vidéo.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">L'article recherché n'existe pas ou n'est pas associé à la vidéo recherchée.</response>
        [HttpDelete("{id}/video/{idvideo}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteVideo(int id, int idvideo)
        {
            var article = await _context.Articles.GetByIdAsync(id);
            if (article is null) return NotFound();

            var video = article.Videos.FirstOrDefault(v => v.Id == idvideo);
            if (video is null) return NotFound();

            article.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Retourne la liste des photos.
        /// </summary>
        /// <returns>La liste des photos.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("photos")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
        {
            return await _context.Photos.ToListAsync();
        }

        /// <summary>
        /// Retourne une photo.
        /// </summary>
        /// <param name="id">L'id de la photo.</param>
        /// <returns>La photo recherchée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La photo recherchée n'existe pas.</response>
        [HttpGet("photos/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            return photo is null ? NotFound() : Ok(photo);
        }

        /// <summary>
        /// Supprime une photo.
        /// </summary>
        /// <param name="id">L'id de la photo.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La photo recherchée n'existe pas.</response>
        [HttpDelete("photos/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo is null) return NotFound();

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retourne la liste des vidéos.
        /// </summary>
        /// <returns>La liste des vidéos.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("videos")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        /// <summary>
        /// Retourne une vidéo.
        /// </summary>
        /// <param name="id">L'id de la vidéo.</param>
        /// <returns>La vidéo recherchée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La vidéo recherchée n'existe pas.</response>
        [HttpGet("videos/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> GetVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            return video is null ? NotFound() : Ok(video);
        }

        /// <summary>
        /// Supprime une vidéo.
        /// </summary>
        /// <param name="id">L'id de la vidéo.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La vidéo recherchée n'existe pas.</response>
        [HttpDelete("videos/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video is null) return NotFound();

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
