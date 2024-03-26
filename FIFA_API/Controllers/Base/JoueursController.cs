using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using NuGet.Protocol.Core.Types;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace FIFA_API.Controllers
{

    /// <summary>
    /// Controleur pour gérer les opérations sur les joueurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JoueursController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly IJoueurManager _joueurManager;
        private readonly IFaqJoueurManager _faqManager;

        public JoueursController(IJoueurManager manager)
        {
            _joueurManager = manager;
        }

        #region Joueurs
        // GET: api/Joueurs
        /// <summary>
        /// Recupere la liste des joueurs.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joueur>>> GetJoueurs()
        {
            var joueurs = await _joueurManager.GetAllAsync();
            return Ok(joueurs);
        }


        // GET: api/Joueurs/5
        /// <summary>
        /// Recupere un joueur par son ID.
        /// </summary>
        /// <param name="id">L'ID du joueur à recuperer.</param>
        /// <returns>Le joueur correspondant à l'ID.</returns>
        [HttpGet("{id}")]
        [ActionName("GetJoueurById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Joueur>> GetJoueurById(int id)
        {
            var joueur = await _joueurManager.GetByIdAsync(id);

            if (joueur is null)
            {
                return NotFound();
            }

            return joueur;


        }

        // PUT: api/Joueurs/5
        /// <summary>
        /// Met à jour un joueur existant.
        /// </summary>
        /// <param name="id">L'ID du joueur à mettre à jour.</param>
        /// <param name="joueur">Les nouvelles données mis à jour du joueur.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutJoueur(int id, Joueur joueur)
        {
            if (id != joueur.Id)
            {
                return BadRequest();
            }

            var oldJoueur = await _joueurManager.GetByIdAsync(id);
            if (oldJoueur is null) return NotFound();

            await _joueurManager.UpdateAsync(joueur);
            return NoContent();
        }

        // POST: api/Joueurs
        /// <summary>
        /// Ajoute un nouveau joueur.
        /// </summary>
        /// <param name="joueur">Les données du joueur à ajouter.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Joueur>> PostJoueur(Joueur joueur)
        {
            if(joueur.Stats is null)
            {
                joueur.Stats = new Statistiques()
                {
                    MinutesJouees = 0,
                    Buts = 0,
                    MatchsJoues = 0,
                    Titularisations = 0
                };
            }

            joueur.Stats.IdJoueur = joueur.Id;

            await _joueurManager.AddAsync(joueur);
            return CreatedAtAction("GetJoueurById", new { id = joueur.Id }, joueur);
        }

        // DELETE: api/Joueurs/5
        /// <summary>
        /// Supprime un joueur existant.
        /// </summary>
        /// <param name="id">L'ID du joueur à supprimer.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteJoueur(int id)
        {
            var joueur = await _joueurManager.GetByIdAsync(id);

            if (joueur is null)
            {
                return NotFound();
            }

            joueur.Stats = null;
            await _joueurManager.SaveChangesAsync();
            await _joueurManager.DeleteAsync(joueur);
            return NoContent();
        }
        #endregion

        #region Faq
        [HttpGet("Faq")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<FaqJoueur>>> GetFaqJoueurs()
        {
            return Ok(await _faqManager.GetAllAsync());
        }

        [HttpGet("Faq/{id}")]
        [ActionName("GetFaqJoueurById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FaqJoueur>> GetFaqJoueurById([FromRoute] int id)
        {
            FaqJoueur? faq = await _faqManager.GetByIdAsync(id);
            return faq is null ? NotFound() : faq;
        }

        [HttpPost("Faq")]
        [Authorize(Policy = MANAGER_POLICY)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FaqJoueur>> AddFaqJoueur([FromBody] FaqJoueur faqJoueur)
        {
            await _faqManager.AddAsync(faqJoueur);
            return CreatedAtAction("GetFaqJoueurById", new { faqJoueur.Id }, faqJoueur);
        }

        [HttpPut("Faq/{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutFaqJoueur([FromRoute] int id, [FromBody] FaqJoueur faqJoueur)
        {
            FaqJoueur? existing = await _faqManager.GetByIdAsync(id);
            if(existing is null)
            {
                return NotFound();
            }

            await _faqManager.UpdateAsync(faqJoueur);
            return NoContent();
        }

        [HttpDelete("Faq")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteFaqJoueur([FromRoute] int id)
        {
            FaqJoueur? faq = await _faqManager.GetByIdAsync(id);
            if(faq is null)
            {
                return NotFound();
            }

            await _faqManager.DeleteAsync(faq);
            return NoContent();
        }
        #endregion
    }
}
