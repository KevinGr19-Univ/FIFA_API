using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FIFA_API.Models.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public DbSet<T> DbSet => dbContext.Set<T>();
        protected readonly FifaDbContext dbContext;

        public BaseRepository(FifaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T elementToAdd)
        {
            await DbSet.AddAsync(elementToAdd);
            await SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T elementToUpdate)
        {
            DbSet.Update(elementToUpdate);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T elementToDelete)
        {
            DbSet.Remove(elementToDelete);
            await SaveChangesAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
