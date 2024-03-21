using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_album_alb")]
    public partial class Album : Publication
    {
        public Album()
        {
            Photos = new HashSet<Photo>();
        }

        [ManyToMany("_albums")]
        public ICollection<Photo> Photos { get; set; }
    }
}
