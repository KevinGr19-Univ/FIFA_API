using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_club_clb")]
    public class Club
    {
		[Column("clb_nom")]
        public string Nom { get; set; }


    }
}
