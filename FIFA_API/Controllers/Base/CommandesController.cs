using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CommandesController : ControllerBase
    {
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IConfiguration _config;
        private readonly FifaDbContext _context;

        public CommandesController(FifaDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Commandes
        [HttpGet]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            return await _context.Commandes.ToListAsync();
        }

        // GET: api/Commandes/5
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<Commande>> GetCommande(int id)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var commande = await _context.Commandes.GetByIdAsync(id);

            if (commande is null) return NotFound();
            if(!User.IsInRole(MANAGER_POLICY) && commande.Id != user.Id) return NotFound(); 

            return commande;
        }

        // PUT: api/Commandes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (id != commande.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(commande);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Commandes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
            await _context.Commandes.AddAsync(commande);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommande", new { id = commande.Id }, commande);
        }

        // DELETE: api/Commandes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandeExists(int id)
        {
            return (_context.Commandes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
