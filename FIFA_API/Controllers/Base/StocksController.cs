using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;
using FIFA_API.Models;

namespace FIFA_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class StocksController : ControllerBase
    {
        public const string EDIT_POLICY = Policies.Admin;
        public const string DELETE_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public StocksController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Stocks
        [HttpGet]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<ActionResult<IEnumerable<StockProduit>>> GetStockProduits()
        {
            return await _context.StockProduits.ToListAsync();
        }

        // GET: api/Stocks/5
        [HttpGet("{idvariante}/{idtaille}")]
        public async Task<ActionResult<StockProduit>> GetStockProduit(int idvariante, int idtaille)
        {
            var stockProduit = await _context.StockProduits.FindAsync(idvariante, idtaille);

            if (stockProduit == null)
            {
                return NotFound();
            }

            return stockProduit;
        }

        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idvariante}/{idtaille}")]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PutStockProduit(int idvariante, int idtaille, [FromBody] StockProduit stockProduit)
        {
            if (idvariante != stockProduit.IdVCProduit || idtaille != stockProduit.IdTaille)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(stockProduit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockProduitExists(idvariante, idtaille))
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

        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<ActionResult<StockProduit>> PostStockProduit(StockProduit stockProduit)
        {
            await _context.StockProduits.AddAsync(stockProduit);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StockProduitExists(stockProduit.IdVCProduit, stockProduit.IdTaille))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStockProduit", new { idvariante = stockProduit.IdVCProduit, idtaille = stockProduit.IdTaille }, stockProduit);
        }

        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        [Authorize(Policy = DELETE_POLICY)]
        public async Task<IActionResult> DeleteStockProduit(int id)
        {
            if (_context.StockProduits == null)
            {
                return NotFound();
            }
            var stockProduit = await _context.StockProduits.FindAsync(id);
            if (stockProduit == null)
            {
                return NotFound();
            }

            _context.StockProduits.Remove(stockProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockProduitExists(int idvariante, int idtaille)
        {
            return (_context.StockProduits?.Any(e => e.IdVCProduit == idvariante && e.IdTaille == idtaille)).GetValueOrDefault();
        }
    }
}
