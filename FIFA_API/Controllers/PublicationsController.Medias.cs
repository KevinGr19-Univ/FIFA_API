using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class PublicationsController
    {
        [HttpPost("{id}/photo")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> PostPhoto(int id, Photo photo)
        {
            var publication = await _context.Publications.GetByIdAsync(id);
            if (publication is null) return NotFound();

            return await PostPhoto(publication, photo);
        }

        [HttpPost("{id}/photo/{idphoto}")]
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

        [HttpDelete("{id}/photo/{idphoto}")]
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

        [HttpPost("{id}/video")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Video>> PostVideo(int id, Video video)
        {
            var article = await _context.Articles.GetByIdAsync(id);
            if (article is null) return NotFound();

            return await PostVideo(article, video);
        }

        [HttpPost("{id}/video/{idvideo}")]
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

        [HttpDelete("{id}/video/{idvideo}")]
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

        [HttpGet("photos/{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            return photo is null ? NotFound() : Ok(photo);
        }

        [HttpDelete("photos/{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo is null) return NotFound();

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("videos/{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Photo>> GetVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            return video is null ? NotFound() : Ok(video);
        }

        [HttpDelete("videos/{id}")]
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
