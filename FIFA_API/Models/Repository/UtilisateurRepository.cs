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
            return await DbSet.FirstOrDefaultAsync(u => u.Mail == email);
        }
    }
}
