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

        /// <summary>
        /// Filtre des entités <see cref="IVisible"/> pour ne voir que celles qui sont visibles.
        /// </summary>
        /// <typeparam name="T">Le type d'entité.</typeparam>
        /// <returns>Une requête retournant les entités visibles.</returns>
        public static IQueryable<T> FilterVisibles<T>(this IQueryable<T> query) where T : IVisible
        {
            return query.Where(v => v.Visible);
        }

        #region Utilisateurs
        /// <summary>
        /// Retourne l'utilisateur associé à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id de l'utilisateur.</param>
        /// <returns>L'utilisateur associé, <see langword="null"/> sinon.</returns>
        public static async Task<Utilisateur?> GetByIdAsync(this IQueryable<Utilisateur> query, int id)
        {
            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Retourne l'utilisateur associé à l'adresse mail.
        /// </summary>
        /// <param name="email">L'adresse mail de l'utilisateur.</param>
        /// <returns>L'utilisateur associé, <see langword="null"/> sinon.</returns>
        public static async Task<Utilisateur?> GetByEmailAsync(this IQueryable<Utilisateur> query, string email)
        {
            return await query.FirstOrDefaultAsync(u => u.Mail == email);
        }

        /// <summary>
        /// Vérifie si une adresse mail est déjà prise.
        /// </summary>
        /// <param name="email">L'adresse mail à vérifier.</param>
        /// <returns><see langword="true"/> si l'adresse mail est déjà prise, <see langword="false"/> sinon.</returns>
        public static async Task<bool> IsEmailTaken(this IQueryable<Utilisateur> query, string email)
        {
            return await query.AnyAsync(u => u.Mail == email);
        }
        #endregion

        #region Commande
        /// <summary>
        /// Trie une liste de commande par date.
        /// </summary>
        /// <param name="desc">Si le tri doit être décroissant.</param>
        /// <returns>Une requête retournant les commandes triées par date.</returns>
        public static IQueryable<Commande> Sort(this IQueryable<Commande> query, bool desc)
        {
            return desc ? query.OrderByDescending(c => c.DateCommande) : query.OrderBy(c => c.DateCommande);
        }

        /// <summary>
        /// Convertit une requête de commandes en aperçus, prêts à être envoyés.
        /// </summary>
        /// <returns>La liste des aperçus de commandes de la requête.</returns>
        public static async Task<IEnumerable<ApercuCommande>> ToApercus(this IQueryable<Commande> query)
        {
            return (await query.ToListAsync()).Select(c => ApercuCommande.FromCommande(c));
        }

        /// <summary>
        /// Retourne la commande associé à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id de la commande.</param>
        /// <returns>la commande associée, <see langword="null"/> sinon.</returns>
        public static async Task<Commande?> GetByIdAsync(this IQueryable<Commande> query, int id)
        {
            return await query.Include(c => c.Status)
                .Include(c => c.Lignes)
                .Include(c => c.TypeLivraison)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Produit
        /// <summary>
        /// Retourne le produit associé à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id du produit.</param>
        /// <returns>Le produit associé, <see langword="null"/> sinon.</returns>
        public static async Task<Produit?> GetByIdAsync(this IQueryable<Produit> query, int id)
        {
            return await query.Include(p => p.Variantes).ThenInclude(v => v.Stocks)
                .Include(p => p.Variantes).ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Retourne la variante de produit associée à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id de la variante de produit.</param>
        /// <returns>La variante de produit associée, <see langword="null"/> sinon.</returns>
        public static async Task<VarianteCouleurProduit?> GetByIdAsync(this IQueryable<VarianteCouleurProduit> query, int id)
        {
            return await query.Include(v => v.Produit)
                .Include(v => v.Couleur)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        #endregion

        #region Joueurs
        /// <summary>
        /// Retourne le joueur associé à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id du joueur.</param>
        /// <returns>Le joueur associé, <see langword="null"/> sinon.</returns>
        public static async Task<Joueur?> GetByIdAsync(this IQueryable<Joueur> query, int id)
        {
            return await query.Include(j => j.Trophees).Include(j => j.FaqJoueurs)
                .Include(j => j.Club)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        /// <summary>
        /// Retourne le joueur associé à l'id, ainsi que les entités associées et ses publications.
        /// </summary>
        /// <param name="id">L'id du joueur.</param>
        /// <returns>Le joueur associé et ses publications, <see langword="null"/> sinon.</returns>
        public static async Task<Joueur?> GetByIdWithPublications(this IQueryable<Joueur> query, int id)
        {
            return await query.Include(j => j.Publications).SingleOrDefaultAsync(j => j.Id == id);
        }
        #endregion

        #region Publications
        /// <summary>
        /// Retourne la publication associée à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id de la publication.</param>
        /// <returns>Le publication associée, <see langword="null"/> sinon.</returns>
        public static async Task<T?> GetByIdAsync<T>(this IQueryable<T> query, int id)
            where T : Publication
        {
            query = query.Include(p => p.Photo);

            if (query is IQueryable<Album> alQuery)
                query = (IQueryable<T>) alQuery.Include(a => a.Photos);

            else if (query is IQueryable<Article> arQuery)
                query = (IQueryable<T>) arQuery.Include(a => a.Photos).Include(a => a.Videos);

            else if (query is IQueryable<Blog> bQuery)
                query = (IQueryable<T>) bQuery.Include(a => a.Photos).Include(b => b.Commentaires);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Themes
        /// <summary>
        /// Retourne le thème de vote associé à l'id, ainsi que les entités associées.
        /// </summary>
        /// <param name="id">L'id du thème de vote.</param>
        /// <returns>Le thème de vote associé, <see langword="null"/> sinon.</returns>
        public static async Task<ThemeVote?> GetByIdAsync(this IQueryable<ThemeVote> query, int id)
        {
            return await query.Include(t => t.AssocJoueurs).FirstOrDefaultAsync(t => t.Id == id);
        }
        #endregion
    }
}
