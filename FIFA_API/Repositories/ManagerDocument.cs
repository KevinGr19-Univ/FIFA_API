using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerDocument : BaseVisibleManager<Document>, IManagerDocument
    {
        public ManagerDocument(DbContext context) : base(context) { }
    }
}
