using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_album_alb")]
    public class Album : Publication
    {
        [Column("pht_id")]
        public int IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        public ICollection<Photo> Photos { get; set; }
    }
}
