using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IUtilisateurRepository dataRepository;

        public UtilisateursController(IUtilisateurRepository dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await dataRepository.GetAllAsync();
        }

        // GET: api/Utilisateurs/GetUtilisateurById
        /// <summary>
        /// Récupère un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur à recuperer.</param>
        /// <returns>L'utilisateur correspondant à l'ID.</returns>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        /// <response code="200">Retourne l'utilisateur.</response>
        [HttpGet("{id}")]
        [ActionName("GetUtilisateurById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
        // GET: api/Utilisateurs/GetUtilisateurByEmail
        /// <summary>
        /// Récupère un utilisateur avec son email.
        /// </summary>
        /// <param name="email">L'email de l'utilisateur à recuperer.</param>
        /// <returns>L'utilisateur correspondant à l'email.</returns>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        /// <response code="200">Retourne l'utilisateur.</response>
        [HttpGet("{email}")]
        [ActionName("GetUtilisateurByEmail")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Utilisateur>> GetByEmailAsync(string email)
        {
            var result = await dataRepository.GetByEmailAsync(email);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Utilisateurs/5
        /// <summary>
        /// Met à jour un utilisateur existant.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'ID de l'utilisateur à mettre à jour.</param>
        /// <param name="utilisateur">Les nouvelles données mis à jour de l'utilisateur.</param>
        /// <response code="204">Quand l'utilisateur est mis à jour.</response>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        /// <response code="400">Mauvaise requête, quand l'ID utilisateur ne correspond pas à l'ID donné.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return BadRequest();
            }

            var userToUpdate = await dataRepository.GetByIdAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(userToUpdate.Value, utilisateur);
                return NoContent();
            }
        }

        // POST: api/Utilisateurs
        /// <summary>
        /// Ajoute un nouvel utilisateur.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="utilisateur">Les données de l'utilisateur à ajouter.</param>
        /// <response code="201">Création de l'utilisateur.</response>
        /// <response code="400">Mauvaise requête, erreur dans l'un des paramètres.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await dataRepository.AddAsync(utilisateur);
            return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.Id }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        /// <summary>
        /// Supprime un joueur existant.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'ID de l'utilisateur à supprimer.</param>
        /// <response code="204">Quand l'utilisateur est supprimé.</response>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }

        //private bool UtilisateurExists(int id)
        //{
        //    return (_context.Utilisateurs?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
