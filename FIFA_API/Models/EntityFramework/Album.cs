using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_album_alb")]
    public partial class Album : Publication
    {
        [ManyToMany("_albums")]
        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
