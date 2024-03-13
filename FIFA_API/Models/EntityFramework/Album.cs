using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_album_alb")]
    public class Album : Publication
    {
        public Album()
        {
            Photos = new HashSet<Photo>();
        }

        public ICollection<Photo> Photos { get; set; }
    }
}
