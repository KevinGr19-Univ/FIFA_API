using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerAlbum : BaseVisibleManager<Album>, IManagerAlbum
    {
        public ManagerAlbum(FifaDbContext context) : base(context) { }

        public async Task<Album?> GetByIdWithPhotos(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
