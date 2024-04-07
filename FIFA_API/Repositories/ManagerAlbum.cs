using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerAlbum : BaseVisibleManager<Album>, IManagerAlbum
    {
        public ManagerAlbum(DbContext context) : base(context) { }
    }
}