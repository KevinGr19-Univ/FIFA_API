using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        [HttpPost("{id}/tailles/{idtaille}")]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PostProduitTaille(int id, int idtaille)
        {
            var produit = await _context.Produits.GetByIdAsync(id);
            if (produit is null) return NotFound();

            var taille = await _context.TailleProduits.FindAsync(idtaille);
            if (taille is null) return NotFound();

            produit.Tailles.Add(taille);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/tailles/{idtaille}")]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> DeleteProduitTaille(int id, int idtaille)
        {
            var produit = await _context.Produits.GetByIdAsync(id);
            if (produit is null) return NotFound();

            var taille = produit.Tailles.FirstOrDefault(t => t.Id == idtaille);
            if (taille is null) return NotFound();

            bool stocksExists = await _context.StockProduits.Include(s => s.VCProduit)
                .AnyAsync(s => s.IdTaille == idtaille && s.VCProduit.IdProduit == id);

            if (stocksExists) return Forbid();

            produit.Tailles.Remove(taille);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
