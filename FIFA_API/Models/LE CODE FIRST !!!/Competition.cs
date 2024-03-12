using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_competition_cmp")]
    public class Competition
    {
		[Column("cmp_nom")]
        public string Nom { get; set; }


        public ICollection<Produit> Produits { get; set; }
    }
}
