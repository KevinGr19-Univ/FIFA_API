using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerBlog : IVisibleRepository<Blog>
    {
        Task<Blog?> GetByIdWithPhotos(int id, bool onlyVisible = true);
        Task<Blog?> GetByIdWithAll(int id, bool onlyVisible = true);
    }
}
