using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_photo_pht")]
    public class Photo
    {
		[Column("pht_nom")]
        public string Nom { get; set; }

		[Column("pht_url")]
        public string Url { get; set; }

    }
}
