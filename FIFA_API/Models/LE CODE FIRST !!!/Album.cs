using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_album_alb")]
    public class Album : Publication
    {
        public ICollection<Photo> Photos { get; set; }
    }
}
