using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerNation : BaseVisibleManager<Nation>, IManagerNation
    {
        public ManagerNation(DbContext context) : base(context) { }
    }
}
