using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerAlbum : IVisibleRepository<Album>
    {
        Task<Album?> GetByIdWithPhotos(int id, bool onlyVisible = true);
    }
}
