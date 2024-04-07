using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerPublication : IVisibleRepository<Publication>
    {
        Task<Publication?> GetByIdWithPhoto(int id, bool onlyVisible = true);
    }
}
