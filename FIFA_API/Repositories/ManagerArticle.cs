using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerArticle : BaseVisibleManager<Article>, IManagerArticle
    {
        public ManagerArticle(FifaDbContext context) : base(context) { }

        public async Task<Article?> GetByIdWithAll(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                 .Include(p => p.Photos)
                 .Include(a => a.Videos)
                 .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Article?> GetByIdWithPhotos(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Article?> GetByIdWithVideos(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(a => a.Videos)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}
