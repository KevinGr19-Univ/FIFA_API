using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static partial class DbSetExtensions
    {
        // TODO : Pagination PAS OPTI
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page-1) * pageSize).Take(pageSize);
        }

        #region Utilisateurs
        public static async Task<Utilisateur?> GetByIdAsync(this IQueryable<Utilisateur> query, int id)
        {
            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        public static async Task<Utilisateur?> GetByEmailAsync(this IQueryable<Utilisateur> query, string email)
        {
            return await query.FirstOrDefaultAsync(u => u.Mail == email);
        }

        public static async Task<bool> IsEmailTaken(this IQueryable<Utilisateur> query, string email)
        {
            return await query.AnyAsync(u => u.Mail == email);
        }
        #endregion

        #region Commande
        public static IQueryable<Commande> Sort(this IQueryable<Commande> query, bool desc)
        {
            return desc ? query.OrderByDescending(c => c.DateCommande) : query.OrderBy(c => c.DateCommande);
        }

        public static async Task<IEnumerable<ApercuCommande>> ToApercus(this IQueryable<Commande> query)
        {
            return (await query.ToListAsync()).Select(c => ApercuCommande.FromCommande(c));
        }

        public static async Task<Commande?> GetByIdAsync(this IQueryable<Commande> query, int id)
        {
            return await query.Include(c => c.Status)
                .Include(c => c.Lignes)
                .Include(c => c.TypeLivraison)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Produit
        public static async Task<Produit?> GetByIdAsync(this IQueryable<Produit> query, int id)
        {
            return await query.Include(p => p.Variantes).ThenInclude(v => v.Stocks)
                .Include(p => p.Variantes).ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public static async Task<VarianteCouleurProduit?> GetByIdAsync(this IQueryable<VarianteCouleurProduit> query, int id)
        {
            return await query.Include(v => v.Produit)
                .Include(v => v.Couleur)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        #endregion

        #region Joueurs
        public static async Task<Joueur?> GetByIdAsync(this IQueryable<Joueur> query, int id)
        {
            return await query.Include(j => j.Trophees).Include(j => j.FaqJoueurs)
                .Include(j => j.Club)
                .FirstOrDefaultAsync(j => j.Id == id);
        }
        #endregion

        #region Publications
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
        public static async Task<ThemeVote?> GetByIdAsync(this IQueryable<ThemeVote> query, int id)
        {
            return await query.Include(t => t.AssocJoueurs).FirstOrDefaultAsync(t => t.Id == id);
        }
        #endregion
    }
}
