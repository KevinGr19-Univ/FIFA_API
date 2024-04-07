using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerCompetition : BaseVisibleManager<Competition>, IManagerCompetition
    {
        public ManagerCompetition(DbContext context) : base(context) { }
    }
}
