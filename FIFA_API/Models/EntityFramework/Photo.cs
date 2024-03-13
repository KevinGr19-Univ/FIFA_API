using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_photo_pht")]
    public class Photo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pht_id")]
        public int Id { get; set; }
        [Column("pht_nom")]
        public string Nom { get; set; }

		[Column("pht_url")]
        public string Url { get; set; }

    }
}