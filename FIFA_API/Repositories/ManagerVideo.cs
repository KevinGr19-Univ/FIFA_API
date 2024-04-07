using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerVideo : BaseManager<Video>, IManagerVideo
    {
        public ManagerVideo(FifaDbContext context) : base(context) { }

        public async Task<Video?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }
    }
}
