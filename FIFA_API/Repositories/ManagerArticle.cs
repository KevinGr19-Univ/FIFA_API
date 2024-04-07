using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerArticle : BaseVisibleManager<Article>, IManagerArticle
    {
        public ManagerArticle(DbContext context) : base(context) { }
    }
}
