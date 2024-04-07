using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerBlog : BaseVisibleManager<Blog>, IManagerBlog
    {
        public ManagerBlog(FifaDbContext context) : base(context) { }

        public async Task<Blog?> GetByIdWithAll(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                .Include(p => p.Photos)
                .Include(p => p.Commentaires)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Blog?> GetByIdWithPhotos(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
