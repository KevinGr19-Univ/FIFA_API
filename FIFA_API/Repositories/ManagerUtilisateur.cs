using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FIFA_API.Repositories
{
    public sealed class ManagerUtilisateur : BaseManager<Utilisateur>, IManagerUtilisateur
    {
        public ManagerUtilisateur(FifaDbContext context) : base(context) { }

        public async Task<Utilisateur?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<Utilisateur?> GetByEmail(string mail)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Mail == mail);
        }

        public async Task<Utilisateur?> GetBy2FAToken(string token2fa)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Token2FA == token2fa);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }

        public async Task<bool> IsEmailTaken(string mail)
        {
            return await DbSet.AnyAsync(e => e.Mail == mail);
        }
    }
}
