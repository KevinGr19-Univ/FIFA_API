using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_album_alb")]
    public partial class Album : Publication
    {
        public Album() { }
        public Album(ILazyLoader loader) : base(loader) { }

        private ICollection<Photo> _photos = new HashSet<Photo>();

        [ManyToMany("_albums")]
        public ICollection<Photo> Photos
        {
            get => loader.Load(this, ref _photos);
            set => _photos = value;
        }
    }
}
