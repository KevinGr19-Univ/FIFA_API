
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerCouleur : BaseVisibleManager<Couleur>, IManagerCouleur
    {
        public ManagerCouleur(DbContext context) : base(context) { }
    }
}
