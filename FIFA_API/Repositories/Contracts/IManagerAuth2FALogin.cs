using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerAuth2FALogin : IRepository<Auth2FALogin>, IGetEntity<int, Auth2FALogin>
    {
    }
}
