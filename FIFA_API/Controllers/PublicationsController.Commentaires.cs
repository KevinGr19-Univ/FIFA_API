using FIFA_API.Authorization;
using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class PublicationsController
    {
        [HttpGet("commentaires")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<CommentaireBlog>>> GetCommentaires()
        {
            return await _context.Commentaires.ToListAsync();
        }

        [HttpGet("commentaires/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentaireBlog>> GetCommentaire(int id)
        {
            var commentaire = await _context.Commentaires.FindAsync(id);
            return commentaire is null ? NotFound() : Ok(commentaire);
        }

        [HttpPost("blogs/{idblog}/comment")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<ActionResult<CommentaireBlog>> PostCommentaire(int idblog, CommentaireBlog commentaire)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var blog = await _context.Blogs.FindAsync(idblog);
            if (blog is null) return NotFound();

            commentaire.Id = 0;
            commentaire.IdUtilisateur = user.Id;
            commentaire.IdBlog = idblog;
            commentaire.EstReponse = commentaire.IdOriginal is not null;
            commentaire.Date = DateTime.Now;

            await _context.Commentaires.AddAsync(commentaire);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCommentaire", new { id = commentaire.Id }, commentaire);
        }

        [HttpDelete("commentaires/{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<IActionResult> DeleteCommentaire(int id, [FromServices] IAuthorizationService authService)
        {
            Utilisateur? user = await this.UtilisateurAsync();

            var commentaire = await _context.Commentaires.FindAsync(id);
            if (commentaire is null) return NotFound();

            if(user is null || commentaire.Id != user.Id)
            {
                var authRes = await authService.AuthorizeAsync(User, MANAGER_POLICY);
                if (!authRes.Succeeded) return Unauthorized();
            }

            _context.Commentaires.Remove(commentaire);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool CommentaireExists(int id)
        {
            return _context.Commentaires.Any(c => c.Id == id);
        }
    }
}
