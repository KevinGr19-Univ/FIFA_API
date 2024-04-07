﻿using Microsoft.AspNetCore.Mvc;
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
    public partial class ArticlesController : ControllerBase
    {
        private readonly IUnitOfWorkPublication _uow;

        public ArticlesController(IUnitOfWorkPublication uow)
        {
            _uow = uow;
        }

        // GET BY ID
        /// <summary>
        /// Retourne une publication de type article.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>La publication de type article recherchée.</returns>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type article ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(PublicationsController.MANAGER_POLICY);
            var article = await _uow.Articles.GetByIdWithAll(id, !seeAll);

            return article is null ? NotFound() : Ok(article);
        }

        // POST
        /// <summary>
        /// Ajoute une publication de type article.
        /// </summary>
        /// <param name="article">La publication à ajouter.</param>
        /// <returns>La nouvelle publication de type article.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La publication de type article est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            await _uow.Articles.Add(article);
            await _uow.SaveChanges();
            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // PUT
        /// <summary>
        /// Modifie une publication de type article.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <param name="article">Les nouvelles informations de la publication.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La publication recherchée n'existe pas, n'est pas de type article ou a été filtrée.</response>
        /// <response code="400">Les nouvelles informations de la publication sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PublicationsController.MANAGER_POLICY)]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id) return BadRequest();

            try
            {
                await _uow.Articles.Update(article);
                await _uow.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!await _uow.Articles.Exists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }
    }
}