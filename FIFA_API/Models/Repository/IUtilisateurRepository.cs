using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Models.Repository
{
    public interface IUtilisateurRepository : IRepository<Utilisateur>
    {
        Task<ActionResult<Utilisateur?>> GetByEmailAsync(string email);
    }
}
