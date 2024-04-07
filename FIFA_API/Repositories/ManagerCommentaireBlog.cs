using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerCommentaireBlog : BaseManager<CommentaireBlog>, IManagerCommentaireBlog
    {
        public ManagerCommentaireBlog(FifaDbContext context) : base(context) { }

        public override async Task<IEnumerable<CommentaireBlog>> GetAll()
        {
            return await DbSet.Include(c => c.Utilisateur)
                .ToListAsync();
        }

        public async Task<CommentaireBlog?> GetById(int key)
        {
            return await DbSet.Include(c => c.Utilisateur)
                .SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }
    }
}
