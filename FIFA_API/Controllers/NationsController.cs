using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;
using NuGet.Protocol.Core.Types;

namespace FIFA_API.Controllers
{
    /// <summary>
    /// Controleur pour gérer les opérations sur les nations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NationsController : ControllerBase
    {

        private readonly IRepository<Nation> _repository;

        public NationsController(IRepository<Nation> repository)
        {
            _repository = repository;
        }


        // GET: api/Nations
        /// <summary>
        /// Recupere la liste des nations.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nation>>> GetNations()
        {
            return await _repository.GetAllAsync();
        }


        // GET: api/Nations/5
        /// <summary>
        /// Recupere une nation par son ID.
        /// </summary>
        /// <param name="id">L'ID de la nation à recuperer.</param>
        /// <returns>La nation correspondant à l'ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Nation>> GetNationById(int id)
        {
            var nation = await _repository.GetByIdAsync(id);

            if (nation == null)
            {
                return NotFound();
            }

            return nation.Value;

        }

        // PUT: api/Nations/5
        /// <summary>
        /// Met à jour une nation existante.
        /// </summary>
        /// <param name="id">L'ID de la nation à mettre à jour.</param>
        /// <param name="nation">Les nouvelles données mis à jour de la nation.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutNation(int id, Nation nation)
        {
            if (id != nation.Id)
            {
                return BadRequest();
            }

            try
            {
                var oldnation = await _repository.GetByIdAsync(id);
                await _repository.UpdateAsync(oldnation.Value, nation);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }



        // POST: api/Nations
        /// <summary>
        /// Ajoute une nouvelle nation.
        /// </summary>
        /// <param name="nation">Les données de la nation à ajouter.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Nation>> PostNation(Nation nation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(nation);

            return CreatedAtAction("GetNationById", new { id = nation.Id }, nation);
        }



        // DELETE: api/Nations/5
        /// <summary>
        /// Supprime une nation existante.
        /// </summary>
        /// <param name="id">L'ID de la nation à supprimer.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteNation(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(result.Value);

            return NoContent();
        }


    }
}
