using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerPublication : BaseVisibleManager<Publication>, IManagerPublication
    {
        public ManagerPublication(DbContext context) : base(context) { }
    }
}
