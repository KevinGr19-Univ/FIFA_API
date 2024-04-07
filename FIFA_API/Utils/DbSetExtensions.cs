using FIFA_API.Models.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static partial class DbSetExtensions
    {
        // TODO : Pagination PAS OPTI
        /// <summary>
        /// Retourne une partie des données de la requête basé sur un système de pages.
        /// </summary>
        /// <typeparam name="T">le type d'entité.</typeparam>
        /// <param name="page">Le numéro de la page à retourner. Commence à 1.</param>
        /// <param name="pageSize">Le nombre d'entités par page.</param>
        /// <returns>Une requête retournant les entités de la page correspondante.</returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page-1) * pageSize).Take(pageSize);
        }
    }
}
