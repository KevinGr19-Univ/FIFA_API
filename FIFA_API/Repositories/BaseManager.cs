using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public abstract class BaseManager<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private FifaDbContext _context;
        public DbSet<TEntity> DbSet => _context.Set<TEntity>();

        public BaseManager(FifaDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public virtual async Task Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public virtual async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
