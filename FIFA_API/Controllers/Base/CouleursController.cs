﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouleursController : ControllerBase
    {
        private readonly IManagerCouleur _manager;

        public CouleursController(IManagerCouleur manager)
        {
            _manager = manager;
        }

        // GET: api/Couleurs
        /// <summary>
        /// Retourne la liste des couleurs.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des couleurs.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Couleur>>> GetCouleurs()
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            return Ok(await _manager.GetAll(!seeAll));
        }

        // GET: api/Couleurs/5
        /// <summary>
        /// Retourne une couleur.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id de la couleur recherchée.</param>
        /// <returns>La couleur recherchée.</returns>
        /// <response code="404">La couleur recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Couleur>> GetCouleur(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var couleur = await _manager.GetById(id, !seeAll);

            if (couleur == null) return NotFound();
            return Ok(couleur);
        }

        // PUT: api/Couleurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=212375
        /// <summary>
        /// Modifie une couleur.
        /// </summary>
        /// <param name="id">L'id de la couleur à modifier.</param>
        /// <param name="couleur">Les nouvelles informations de la couleur.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="404">La couleur recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la couleur sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutCouleur(int id, Couleur couleur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != couleur.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(couleur);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Couleurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une nouvelle couleur.
        /// </summary>
        /// <param name="couleur">La couleur à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>La nouvelle couleur.</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="400">La nouvelle couleur est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<Couleur>> PostCouleur(Couleur couleur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _manager.Add(couleur);
            await _manager.Save();

            return CreatedAtAction("GetCouleur", new { id = couleur.Id }, couleur);
        }

        // DELETE: api/Couleurs/5
        /// <summary>
        /// Supprime une couleur.
        /// </summary>
        /// <param name="id">L'id de la couleur à supprimer.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="404">La couleur recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteCouleur(int id)
        {
            var couleur = await _manager.GetById(id, false);
            if (couleur == null)
            {
                return NotFound();
            }

            await _manager.Delete(couleur);
            await _manager.Save();

            return NoContent();
        }
    }
}
