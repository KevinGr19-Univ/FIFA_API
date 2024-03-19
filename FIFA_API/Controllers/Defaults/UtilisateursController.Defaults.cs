using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Models;
using FIFA_API.Utils;
using FIFA_API.Contracts.Repository;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class UtilisateursController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly IUtilisateurManager _manager;

        public UtilisateursController(IUtilisateurManager manager)
        {
            _manager = manager;
        }


        // GET: api/Utilisateurs
        [HttpGet]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            var users = await _manager.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Utilisateurs/GetUtilisateurById
        /// <summary>
        /// Récupère un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur à recuperer.</param>
        /// <returns>L'utilisateur correspondant à l'ID.</returns>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        /// <response code="200">Retourne l'utilisateur.</response>
        [HttpGet("GetById/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Utilisateur>> GetById(int id)
        {
            var user = await _manager.GetByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Utilisateurs/GetUtilisateurByEmail
        /// <summary>
        /// Récupère un utilisateur avec son email.
        /// </summary>
        /// <param name="email">L'email de l'utilisateur à recuperer.</param>
        /// <returns>L'utilisateur correspondant à l'email.</returns>
        /// <response code="404">Quand l'on ne trouve pas l'utilisateur.</response>
        /// <response code="200">Retourne l'utilisateur.</response>
        [HttpGet("GetByEmail/{email}")]
        [ActionName("GetByEmail")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Utilisateur>> GetByEmail(string email)
        {
            var user = await _manager.GetByEmailAsync(email);
            if (user is null)
            {
                return NotFound();
            }

            return user;
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
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != utilisateur.Id)
            {
                return BadRequest();
            }

            var userToUpdate = await _manager.GetByIdAsync(id);
            if (userToUpdate is null)
            {
                return NotFound();
            }

            if (userToUpdate.Mail != utilisateur.Mail && await _manager.IsEmailTaken(utilisateur.Mail))
            {
                return Forbid();
            }

            await _manager.UpdateAsync(userToUpdate, utilisateur);
            return NoContent();
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
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _manager.IsEmailTaken(utilisateur.Mail))
            {
                return Forbid();
            }

            await _manager.AddAsync(utilisateur);
            return CreatedAtAction(nameof(GetById), new { id = utilisateur.Id }, utilisateur);
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
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var userToDelete = await _manager.GetByIdAsync(id);
            if (userToDelete is null)
            {
                return NotFound();
            }
            await _manager.DeleteAsync(userToDelete);
            return NoContent();
        }
    }
}
