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
        [StringLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

		[Column("pht_url")]
        [StringLength(500, ErrorMessage = "L'url ne doit pas dépasser 500 caractères")]
        public string Url { get; set; }
    }
}
