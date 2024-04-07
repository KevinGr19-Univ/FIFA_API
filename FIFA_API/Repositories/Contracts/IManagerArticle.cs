using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerArticle : IVisibleRepository<Article>
    {
        Task<Article?> GetByIdWithPhotos(int id, bool onlyVisible = true);
        Task<Article?> GetByIdWithVideos(int id, bool onlyVisible = true);
        Task<Article?> GetByIdWithAll(int id, bool onlyVisible = true);
    }
}
