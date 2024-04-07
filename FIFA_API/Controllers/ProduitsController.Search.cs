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
            return Ok(await _uow.SearchProduits(
                q,
                categories,
                competitions,
                genres,
                nations,
                couleurs,
                tailles,
                desc,
                page ?? 1,
                PRODUCTS_PER_PAGE));
        }
    }
}
