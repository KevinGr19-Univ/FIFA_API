using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_categorieproduit_cpr")]
    public class CategorieProduit
    {
		[Column("cpr_nom")]
        public string Nom { get; set; }


        public CategorieProduit? Parent { get; set; }
        public ICollection<CategorieProduit> SousCategories { get; set; }
        public ICollection<Produit> Produits { get; set; }
    }
}
