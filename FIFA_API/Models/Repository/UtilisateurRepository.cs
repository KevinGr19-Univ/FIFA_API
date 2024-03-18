using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class UtilisateurRepository : BaseRepository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<ActionResult<Utilisateur?>> GetByEmailAsync(string email)
        {
            return await IncludeAll(DbSet).FirstOrDefaultAsync(u => u.Mail == email);
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await DbSet.AnyAsync(u => u.Mail == email);
        }

        public override async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await IncludeAll(DbSet).ToListAsync();
        }

        private IQueryable<Utilisateur> IncludeAll(IQueryable<Utilisateur> query)
        {
            return query;
        }
    }
}
