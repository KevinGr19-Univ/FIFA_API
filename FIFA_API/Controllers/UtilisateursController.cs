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
        [HttpGet("{email}")]
        [ActionName("GetUtilisateurByEmail")]
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
        /// Replace an user.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">The id of the user you want to replace</param>
        /// <param name="serie">The values of the replaced user</param>
        /// <response code="204">When the user is replaced</response>
        /// <response code="404">When the user is not found</response>
        /// <response code="400">Bad request, when the user id does not match the given id</response>
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
        /// Create an user.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="serie">The user you want to create</param>
        /// <response code="201">When the user is created</response>
        /// <response code="400">Bad request, when there is an error in one of the parameters</response>
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
        /// Delete an user.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">The id of the user you want to delete</param>
        /// <response code="204">When the user is deleted</response>
        /// <response code="404">When the user is not found</response>
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
