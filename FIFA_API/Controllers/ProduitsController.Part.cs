using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        public const int PRODUCTS_PER_PAGE = 20;

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SearchProductItem>>> SearchProduits(
            [FromQuery] string? q,
            [FromQuery] int[] categories,
            [FromQuery] int[] competitions,
            [FromQuery] int[] genres,
            [FromQuery] int[] nations,
            [FromQuery] int[] couleurs,
            [FromQuery] int[] tailles,
            [FromQuery] bool? desc,
            [FromQuery] int? page)
        {
            var query = _context.Produits
                .Include(p => p.Variantes)
                .Include(p => p.Tailles)
                .Select(p => new
                {
                    Produit = p,
                    Couleurs = p.Variantes.Select(v => v.IdCouleur),
                    Tailles = p.Tailles.Select(t => t.Id)
                });

            if (categories.Length > 0)
                query = query.Where(p => categories.Contains(p.Produit.IdCategorieProduit));

            if (competitions.Length > 0)
                query = query.Where(p => p.Produit.IdCompetition != null &&  competitions.Contains((int)p.Produit.IdCompetition));

            if (genres.Length > 0)
                query = query.Where(p => p.Produit.IdGenre != null && genres.Contains((int)p.Produit.IdGenre));

            if (nations.Length > 0)
                query = query.Where(p => p.Produit.IdNation != null && nations.Contains((int)p.Produit.IdNation));

            if (couleurs.Length > 0)
                query = query.Where(p => p.Couleurs.Any(c => couleurs.Contains(c)));

            if (tailles.Length > 0)
                query = query.Where(p => p.Tailles.Any(t => tailles.Contains(t)));

            if(q is not null)
                query = query.Where(p => p.Produit.Titre.ToLower().Contains(q.ToLower()));

            if (desc == false)
                query = query.OrderBy(p => p.Produit.Variantes.Min(v => v.Prix));

            else if (desc == true)
                query = query.OrderByDescending(p => p.Produit.Variantes.Min(v => v.Prix));

            // TODO : Pagination PAS OPTI
            if (page is not null)
            {
                int _page = (int)page;
                if (_page < 1) return BadRequest(new { Message = "Invalid page" });

                query = query.Skip(PRODUCTS_PER_PAGE * (_page - 1)); 
            }
            query = query.Take(PRODUCTS_PER_PAGE);

            return Ok(await query.Select(p => 
                new {
                    p.Produit,
                    p.Couleurs,
                    p.Tailles,
                    MinVariante = p.Produit.Variantes.OrderBy(v => v.Prix).First()
                }
            ).Select(p => 
                new SearchProductItem
                {
                    Id = p.Produit.Id,
                    Titre = p.Produit.Titre,
                    Prix = p.MinVariante.Prix,
                    Couleurs = p.Couleurs.ToArray(),
                    Tailles = p.Tailles.ToArray(),
                    ImageUrl = p.MinVariante.ImageUrls[0]
                }
            ).ToListAsync());
        }
    }
}
