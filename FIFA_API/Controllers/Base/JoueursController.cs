//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using FIFA_API.Models.EntityFramework;
//using NuGet.Protocol.Core.Types;
//using FIFA_API.Contracts.Repository;

//namespace FIFA_API.Controllers
//{

//    /// <summary>
//    /// Controleur pour gérer les opérations sur les joueurs.
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    public class JoueursController : ControllerBase
//    {
//        private readonly IRepository<Joueur> _repository;

//        public JoueursController(IRepository<Joueur> repository)
//        {
//            _repository = repository;   
//        }

//        // GET: api/Joueurs
//        /// <summary>
//        /// Recupere la liste des joueurs.
//        /// </summary>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Joueur>>> GetJoueurs()
//        {
//            var joueurs = await _repository.GetAllAsync();
//            return Ok(joueurs);
//        }


//        // GET: api/Joueurs/5
//        /// <summary>
//        /// Recupere un joueur par son ID.
//        /// </summary>
//        /// <param name="id">L'ID du joueur à recuperer.</param>
//        /// <returns>Le joueur correspondant à l'ID.</returns>
//        [HttpGet("{id}")]
//        [ActionName("GetJoueurById")]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult<Joueur>> GetJoueurById(int id)
//        {
//            var joueur = await _repository.GetByIdAsync(id);

//            if (joueur == null)
//            {
//                return NotFound();
//            }

//            return joueur;


//        }

//        // PUT: api/Joueurs/5
//        /// <summary>
//        /// Met à jour un joueur existant.
//        /// </summary>
//        /// <param name="id">L'ID du joueur à mettre à jour.</param>
//        /// <param name="joueur">Les nouvelles données mis à jour du joueur.</param>
//        [HttpPut("{id}")]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        public async Task<IActionResult> PutJoueur(int id, Joueur joueur)
//        {

//            if (id != joueur.Id)
//            {
//                return BadRequest();
//            }

//            try
//            {
//                var oldJoueur  = await _repository.GetByIdAsync(id);
//                await _repository.UpdateAsync(oldJoueur, joueur);
//            }
//            catch (Exception)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }

//        // POST: api/Joueurs
//        /// <summary>
//        /// Ajoute un nouveau joueur.
//        /// </summary>
//        /// <param name="joueur">Les données du joueur à ajouter.</param>
//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        public async Task<ActionResult<Joueur>> PostJoueur(Joueur joueur)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            await _repository.AddAsync(joueur);

//            return CreatedAtAction("GetJoueurById", new { id = joueur.Id }, joueur);
//        }

//        // DELETE: api/Joueurs/5
//        /// <summary>
//        /// Supprime un joueur existant.
//        /// </summary>
//        /// <param name="id">L'ID du joueur à supprimer.</param>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        public async Task<IActionResult> DeleteJoueur(int id)
//        {
//            var joueur = await _repository.GetByIdAsync(id);

//            if (joueur == null)
//            {
//                return NotFound();
//            }

//            await _repository.DeleteAsync(joueur);

//            return NoContent();
//        }

//    }
//}
