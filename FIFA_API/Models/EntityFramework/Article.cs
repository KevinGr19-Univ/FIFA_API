using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_article_art")]
    public partial class Article : Publication
    {
        public Article()
        {
            Photos = new HashSet<Photo>();
            Videos = new HashSet<Video>();
        }

        [Column("art_texte", TypeName = "text")]
        public string Texte { get; set; }

        [ManyToMany("_articles")]
        public virtual ICollection<Photo> Photos { get; set; }

        [ManyToMany("_articles")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
