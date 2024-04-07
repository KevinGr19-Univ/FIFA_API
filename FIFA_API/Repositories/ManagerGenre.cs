using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerGenre : BaseVisibleManager<Genre>, IManagerGenre
    {
        public ManagerGenre(DbContext context) : base(context) { }
    }
}
