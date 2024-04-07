using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerCommentaireBlog : IRepository<CommentaireBlog>, IGetEntity<int, CommentaireBlog>
    {
    }
}
