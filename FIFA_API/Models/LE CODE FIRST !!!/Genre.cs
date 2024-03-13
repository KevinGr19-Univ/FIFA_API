using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_genre_gen")]
    public class Genre
    {
        [Key]
        [Column("gen_id")]
        public int Id { get; set; }

        [Column("gen_nom")]
        public string Nom { get; set; }


    }
}
