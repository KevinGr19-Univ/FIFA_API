using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Met à jour une entité et sauvegarde les changements.
        /// </summary>
        /// <typeparam name="TEntity">Le type de l'entité.</typeparam>
        /// <param name="entity">L'entité à mettre à jour.</param>
        public static async Task UpdateEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
