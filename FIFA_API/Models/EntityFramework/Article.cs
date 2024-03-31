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
        public Article() { }
        public Article(ILazyLoader loader) : base(loader) { }

        [Column("art_texte", TypeName = "text"), Required]
        public string Texte { get; set; }

        private ICollection<Photo> _photos = new HashSet<Photo>();
        private ICollection<Video> _videos = new HashSet<Video>();

        [ManyToMany("_articles")]
        public ICollection<Photo> Photos
        {
            get => loader.Load(this, ref _photos);
            set => _photos = value;
        }

        [ManyToMany("_articles")]
        public ICollection<Video> Videos
        {
            get => loader.Load(this, ref _videos);
            set => _videos = value;
        }
    }
}
