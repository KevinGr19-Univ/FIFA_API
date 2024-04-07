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
        [Column("blg_texte", TypeName ="text"), Required]
        public string Texte { get; set; }

        [ManyToMany("_blogs")]
        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();

        [InverseProperty(nameof(CommentaireBlog.Blog))]
        public ICollection<CommentaireBlog> Commentaires { get; set; } = new HashSet<CommentaireBlog>();
    }
}
