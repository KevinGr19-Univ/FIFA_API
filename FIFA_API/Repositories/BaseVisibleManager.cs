using FIFA_API.Models.Contracts;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public abstract class BaseVisibleManager<TEntity> : BaseManager<TEntity>, IVisibleRepository<TEntity> where TEntity : class, IVisible
    {
        protected BaseVisibleManager(DbContext context) : base(context) { }

        public override async Task<IEnumerable<TEntity>> GetAll()
        {
            return await GetAll(true);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(bool onlyVisible = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (onlyVisible) query = query.FilterVisibles();
            return await query.ToListAsync();
        } 

        public virtual async Task<TEntity?> GetById(int id, bool onlyVisible = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (onlyVisible) query = query.FilterVisibles();
            return await query.SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await DbSet.AnyAsync(e => e.Id == id);
        }
        
    }
}
