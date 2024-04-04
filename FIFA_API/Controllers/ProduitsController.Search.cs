using FIFA_API.Models.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        public const int PRODUCTS_PER_PAGE = 20;

        /// <summary>
        /// Recherche et retourne une liste de produits basés sur les critères de recherche.
        /// </summary>
        /// <param name="q">Les mots clés de la recherche.</param>
        /// <param name="categories">Les ids de catégories recherchées.</param>
        /// <param name="competitions">Les ids des compétitions recherchées.</param>
        /// <param name="genres">Les ids des genres recherchées.</param>
        /// <param name="nations">Les ids des nations recherchées.</param>
        /// <param name="couleurs">Les ids des couleurs recherchées.</param>
        /// <param name="tailles">Les ids des tailles recherchées.</param>
        /// <param name="desc">Si les produits doivent être triés par ordre croissant ou décroissant sur leur prix.</param>
        /// <param name="page">Le numéro de page à utiliser pour paginer.</param>
        /// <returns>Une liste de produits correspondants.</returns>
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                .Where(p => p.Visible)
                .Include(p => p.Variantes)
                .ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .Select(p => new
                {
                    Produit = p,
                    Variantes = p.Variantes.Where(c => c.Visible && c.Couleur.Visible),
                    Tailles = p.Tailles.Where(t => t.Visible).Select(t => t.Id)
                })
                .Where(p => p.Variantes.Count() > 0 && p.Tailles.Count() > 0);

            categories = await FilterVisibles(categories, _context.CategorieProduits);
            competitions = await FilterVisibles(competitions, _context.Competitions);
            genres = await FilterVisibles(genres, _context.Genres);
            nations = await FilterVisibles(nations, _context.Nations);
            couleurs = await FilterVisibles(couleurs, _context.Couleurs);
            tailles = await FilterVisibles(tailles, _context.TailleProduits);

            if (categories.Length > 0)
                query = query.Where(p => categories.Contains(p.Produit.IdCategorieProduit));

            if (competitions.Length > 0)
                query = query.Where(p => p.Produit.IdCompetition != null &&  competitions.Contains((int)p.Produit.IdCompetition));

            if (genres.Length > 0)
                query = query.Where(p => p.Produit.IdGenre != null && genres.Contains((int)p.Produit.IdGenre));

            if (nations.Length > 0)
                query = query.Where(p => p.Produit.IdNation != null && nations.Contains((int)p.Produit.IdNation));

            if (couleurs.Length > 0)
                query = query.Where(p => p.Variantes.Any(c => couleurs.Contains(c.IdCouleur)));

            if (tailles.Length > 0)
                query = query.Where(p => p.Tailles.Any(t => tailles.Contains(t)));

            if(q is not null)
                query = query.Where(p => p.Produit.Titre.ToLower().Contains(q.ToLower()));

            if (desc == false)
                query = query.OrderBy(p => p.Produit.Variantes.Min(v => v.Prix));

            else if (desc == true)
                query = query.OrderByDescending(p => p.Produit.Variantes.Min(v => v.Prix));

            query = query.Paginate(Math.Max(page ?? 1, 1), PRODUCTS_PER_PAGE);

            return Ok(await query.Select(p => 
                new {
                    p.Produit,
                    Couleurs = p.Variantes.Select(v => v.IdCouleur),
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

        private async Task<int[]> FilterVisibles(int[] ids, IQueryable<IVisible> visibles)
        {
            if (ids.Length == 0) return ids;
            return await visibles.Where(v => v.Visible && ids.Contains(v.Id)).Select(v => v.Id).ToArrayAsync();
        }
    }
}
