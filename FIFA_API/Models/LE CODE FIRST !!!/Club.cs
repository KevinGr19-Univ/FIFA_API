using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_club_clb")]
    public class Club
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clb_id")]
        public int Id { get; set; }

        [Column("clb_nom")]
        public string Nom { get; set; }


    }
}
