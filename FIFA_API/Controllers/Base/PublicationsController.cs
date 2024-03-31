﻿using System;
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
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public PublicationsController(FifaDbContext context)
        {
            _context = context;
        }

        #region Generic actions
        [NonAction]
        public async Task<ActionResult<IEnumerable<T>>> GetPublications<T>() where T : Publication
        {
            return await _context.Set<T>().ToListAsync();
        }

        [NonAction]
        public async Task<ActionResult<T>> GetPublication<T>(int id) where T : Publication
        {
            var publication = await _context.Set<T>().GetByIdAsync(id);
            if (publication == null) return NotFound();
            return publication;
        }

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
        [HttpGet("albums/{id}")]
        public async Task<ActionResult<Album>> GetAlbum(int id) => await GetPublication<Album>(id);

        [HttpGet("articles/{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id) => await GetPublication<Article>(id);

        [HttpGet("blogs/{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id) => await GetPublication<Blog>(id);

        [HttpGet("documents/{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id) => await GetPublication<Document>(id);

        // POST
        [HttpPost("albums")]
        public async Task<ActionResult<Album>> PostAlbum(Album album) => await PostPublication(album, "GetAlbum");

        [HttpPost("articles")]
        public async Task<ActionResult<Article>> PostArticle(Article article) => await PostPublication(article, "GetArticle");

        [HttpPost("blogs")]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog) => await PostPublication(blog, "GetBlog");

        [HttpPost("documents")]
        public async Task<ActionResult<Document>> PostDocument(Document document) => await PostPublication(document, "GetDocument");

        // PUT
        [HttpPut("albums/{id}")]
        public async Task<IActionResult> PutAlbum(int id, Album album) => await PutPublication(id, album);

        [HttpPut("articles/{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article) => await PutPublication(id, article);

        [HttpPut("blogs/{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blog) => await PutPublication(id, blog);

        [HttpPut("documents/{id}")]
        public async Task<IActionResult> PutDocument(int id, Document document) => await PutPublication(id, document);

        [HttpDelete("{id}")]
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