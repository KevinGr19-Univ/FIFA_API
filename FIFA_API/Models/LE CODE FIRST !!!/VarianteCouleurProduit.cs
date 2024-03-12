using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_variantecouleurproduit_vcp")]
    public class VarianteCouleurProduit
    {
        public Produit Produit { get; set; }
        public Couleur Couleur { get; set; }

		[Column("vcp_prix")]
        public decimal Prix { get; set; }

        public ICollection<string> ImageUrls { get; set; }
    }
}
