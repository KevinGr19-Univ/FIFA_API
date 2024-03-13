using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_album_alb")]
    public class Album : Publication
    {
        [Column("alb_id")]
        public int Id { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
