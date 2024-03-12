using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_typelivraison_tli")]
    public class TypeLivraison
    {
		[Column("tli_nom")]
        public string Nom { get; set; }

		[Column("tli_maxbusinessdays")]
        public int MaxBusinessDays { get; set; }

		[Column("tli_prix")]
        public decimal Prix { get; set; }

    }
}
