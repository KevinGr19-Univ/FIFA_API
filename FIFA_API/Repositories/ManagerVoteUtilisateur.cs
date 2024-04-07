using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerVoteUtilisateur : BaseManager<VoteUtilisateur>, IManagerVoteUtilisateur
    {
        public ManagerVoteUtilisateur(FifaDbContext context) : base(context) { }

        public async Task<VoteUtilisateur?> GetById(int idtheme, int iduser)
        {
            return await DbSet.FindAsync(idtheme, iduser);
        }

        public async Task<bool> Exists(int idtheme, int iduser)
        {
            return await DbSet.AnyAsync(e => e.IdTheme == idtheme && e.IdUtilisateur == iduser);
        }

        public async Task<IEnumerable<VoteUtilisateur>> GetUserVotes(int iduser)
        {
            return await DbSet.Where(e => e.IdUtilisateur == iduser).ToListAsync();
        }
    }
}
