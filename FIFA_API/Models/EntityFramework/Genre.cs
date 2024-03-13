using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_genre_gen")]
    public class Genre
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("gen_id")]
        public int Id { get; set; }

        [Column("gen_nom")]
        public string Nom { get; set; }


    }
}
