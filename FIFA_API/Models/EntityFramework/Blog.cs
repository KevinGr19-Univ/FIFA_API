using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_blog_blg")]
    public partial class Blog : Publication
    {
        public Blog() { }
        public Blog(ILazyLoader loader) : base(loader) { }

        [Column("blg_texte", TypeName ="text"), Required]
        public string Texte { get; set; }

        private ICollection<Photo> _photos = new HashSet<Photo>();

        [ManyToMany("_blogs")]
        public ICollection<Photo> Photos
        {
            get => loader.Load(this, ref _photos);
            set => _photos = value;
        }

        private ICollection<CommentaireBlog> _commentaires = new HashSet<CommentaireBlog>();

        [InverseProperty(nameof(CommentaireBlog.Blog))]
        public ICollection<CommentaireBlog> Commentaires
        {
            get => loader.Load(this, ref _commentaires);
            set => _commentaires = value;
        }
    }
}
