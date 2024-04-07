using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected FifaDbContext context;

        public UnitOfWork(FifaDbContext context)
        {
            this.context = context;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
