using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerBlog : BaseVisibleManager<Blog>, IManagerBlog
    {
        public ManagerBlog(DbContext context) : base(context) { }
    }
}
