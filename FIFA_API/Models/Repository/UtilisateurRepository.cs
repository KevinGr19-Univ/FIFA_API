using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class UtilisateurRepository : BaseRepository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(FifaDbContext dbContext) : base(dbContext) { }

        public override async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await IncludeAll(DbSet).ToListAsync();
        }

        public async Task<ActionResult<Utilisateur?>> GetByEmailAsync(string email)
        {
            return await IncludeAll(DbSet).FirstOrDefaultAsync(u => u.Mail == email);
        }

        private IQueryable<Utilisateur> IncludeAll(IQueryable<Utilisateur> query)
        {
            return query;
        }
    }
}
