using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerDocument : BaseVisibleManager<Document>, IManagerDocument
    {
        public ManagerDocument(FifaDbContext context) : base(context) { }

        public async Task<Document?> GetByIdWithAll(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Photo)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
