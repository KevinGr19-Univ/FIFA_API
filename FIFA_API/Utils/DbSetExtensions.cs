using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static class DbSetExtensions
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
            return await query.Include(p => p.Variantes)
                .ThenInclude(v => v.Stocks)
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
    }
}
