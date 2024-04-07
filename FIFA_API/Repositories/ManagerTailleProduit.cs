using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerTailleProduit : BaseVisibleManager<TailleProduit>, IManagerTailleProduit
    {
        public ManagerTailleProduit(FifaDbContext context) : base(context) { }
    }
}
