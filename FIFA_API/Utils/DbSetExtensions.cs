using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static class DbSetExtensions
    {
        #region Utilisateurs
        public static async Task<Utilisateur?> GetByIdAsync(this DbSet<Utilisateur> dbSet, int id)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public static async Task<Utilisateur?> GetByEmailAsync(this DbSet<Utilisateur> dbSet, string email)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Mail == email);
        }

        public static async Task<bool> IsEmailTaken(this DbSet<Utilisateur> dbSet, string email)
        {
            return await dbSet.AnyAsync(u => u.Mail == email);
        }
        #endregion
    }
}
