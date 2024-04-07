using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_article_art")]
    public partial class Article : Publication
    {
        [Column("art_texte", TypeName = "text"), Required]
        public string Texte { get; set; }

        [ManyToMany("_articles")]
        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();

        [ManyToMany("_articles")]
        public ICollection<Video> Videos { get; set; } = new HashSet<Video>();
    } 
}
