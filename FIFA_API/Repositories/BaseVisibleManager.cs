using FIFA_API.Models.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public abstract class BaseVisibleManager<TEntity> : BaseManager<TEntity>, IVisibleRepository<TEntity> where TEntity : class, IVisible
    {
        protected BaseVisibleManager(FifaDbContext context) : base(context) { }

        public override async Task<IEnumerable<TEntity>> GetAll()
        {
            return await GetAll(true);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).ToListAsync();
        } 

        public virtual async Task<TEntity?> GetById(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await DbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<int[]> FilterVisibleIds(int[] ids)
        {
            if (ids.Length == 0) return ids;
            return await DbSet.Where(v => v.Visible && ids.Contains(v.Id)).Select(v => v.Id).ToArrayAsync();
        }

        protected IQueryable<TEntity> Visibility(IQueryable<TEntity> query, bool onlyVisible)
        {
            if (onlyVisible) return query.Where(e => e.Visible);
            return query;
        }
    }
}
