using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerCategorieProduit : BaseVisibleManager<CategorieProduit>, IManagerCategorieProduit
    {
        public ManagerCategorieProduit(FifaDbContext context) : base(context) { }
    }
}
