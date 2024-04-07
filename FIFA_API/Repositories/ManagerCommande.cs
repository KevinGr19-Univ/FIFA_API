using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FIFA_API.Utils;
using FIFA_API.Models.Controllers;

namespace FIFA_API.Repositories
{
    public sealed class ManagerCommande : BaseManager<Commande>, IManagerCommande
    {
        public ManagerCommande(FifaDbContext context) : base(context) { }

        public async Task<Commande?> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<Commande?> GetByIdWithAll(int id)
        {
            return await DbSet.Include(c => c.Lignes)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Commande?> GetByIdWithStatus(int id)
        {
            return await DbSet.Include(c => c.Status)
                .Include(c => c.Lignes)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Commande?> GetByIdWithLignes(int id)
        {
            return await DbSet.Include(c => c.Status)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }

        public async Task<IEnumerable<ApercuCommande>> SearchCommandes(int? iduser, int[] typesLivraison, bool? desc, int page, int amount)
        {
            IQueryable<Commande> query = DbSet.Include(c => c.Status);
            if (iduser is not null)
                query = query.Where(c => c.IdUtilisateur == iduser);

            if (typesLivraison.Length > 0)
                query = query.Where(c => typesLivraison.Contains(c.IdTypeLivraison));

            query = Sort(query, desc == true)
                .Paginate(page, amount);

            return await ToApercus(query);
        }

        private static IQueryable<Commande> Sort(IQueryable<Commande> query, bool desc)
        {
            return desc ? query.OrderByDescending(c => c.DateCommande) : query.OrderBy(c => c.DateCommande);
        }

        private static async Task<IEnumerable<ApercuCommande>> ToApercus(IQueryable<Commande> query)
        {
            return (await query.ToListAsync()).Select(c => ApercuCommande.FromCommande(c));
        }
    }
}
