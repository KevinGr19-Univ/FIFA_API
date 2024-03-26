using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Utils
{
    public static class DbContextExtensions
    {
        public static async Task UpdateEntity<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
