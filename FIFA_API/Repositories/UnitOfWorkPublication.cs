using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public class UnitOfWorkPublication : UnitOfWork, IUnitOfWorkPublication
    {
        public UnitOfWorkPublication(FifaDbContext context) : base(context) { }

        #region Getters
        private IManagerPublication _publications;
        public IManagerPublication Publications
        {
            get
            {
                if(_publications is null) _publications = new ManagerPublication(context);
                return _publications;
            }
        }

        private IManagerAlbum _albums;
        public IManagerAlbum Albums
        {
            get
            {
                if(_albums is null) _albums = new ManagerAlbum(context);
                return _albums;
            }
        }

        private IManagerArticle _articles;
        public IManagerArticle Articles
        {
            get
            {
                if(_articles is null) _articles = new ManagerArticle(context);
                return _articles;
            }
        }

        private IManagerBlog _blogs;
        public IManagerBlog Blogs
        {
            get
            {
                if(_blogs is null) _blogs = new ManagerBlog(context);
                return _blogs;
            }
        }

        private IManagerDocument _documents;
        public IManagerDocument Documents
        {
            get
            {
                if(_documents is null) _documents = new ManagerDocument(context);
                return _documents;
            }
        }

        private IManagerCommentaireBlog _commentaires;
        public IManagerCommentaireBlog Commentaires
        {
            get
            {
                if(_commentaires is null) _commentaires = new ManagerCommentaireBlog(context);
                return _commentaires;
            }
        }

        private IManagerPhoto _photos;
        public IManagerPhoto Photos
        {
            get
            {
                if(_photos is null) _photos = new ManagerPhoto(context);
                return _photos;
            }
        }

        private IManagerVideo _videos;
        public IManagerVideo Videos
        {
            get
            {
                if (_videos is null) _videos = new ManagerVideo(context);
                return _videos;
            }
        }

        #endregion

    }
}
