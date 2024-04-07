using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerVarianteCouleurProduit : BaseVisibleManager<VarianteCouleurProduit>, IManagerVarianteCouleurProduit
    {
        public ManagerVarianteCouleurProduit(DbContext context) : base(context) { }

        public async Task<VarianteCouleurProduit?> GetByIdWithAll(int id)
        {
            return await DbSet.Include(v => v.Produit)
                .Include(v => v.Couleur)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> CombinationExists(int idproduit, int idcouleur)
        {
            return await DbSet.AnyAsync(e => e.IdProduit == idproduit && e.IdCouleur == idcouleur);
        }
    }
}
