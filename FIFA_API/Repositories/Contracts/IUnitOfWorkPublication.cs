namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkPublication : IUnitOfWork
    {
        IManagerPublication Publications { get; }
        IManagerAlbum Albums { get; }
        IManagerArticle Articles { get; }
        IManagerBlog Blogs { get; }
        IManagerDocument Documents { get; }

        IManagerCommentaireBlog Commentaires { get; }
        IManagerPhoto Photos { get; }
        IManagerVideo Videos { get; }
    }
}
