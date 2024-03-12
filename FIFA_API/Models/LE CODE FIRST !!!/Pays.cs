using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_pays_pys")]
    public class Pays
    {
		[Column("pys_nom")]
        public string Nom { get; set; }


    }
}
