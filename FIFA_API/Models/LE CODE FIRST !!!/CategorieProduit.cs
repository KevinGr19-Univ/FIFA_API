using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_categorieproduit_cpr")]
    public class CategorieProduit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cpr_id")]
        public int Id { get; set; }

        [Column("cpr_nom")]
        public string Nom { get; set; }


        public CategorieProduit? Parent { get; set; }

        public ICollection<CategorieProduit> SousCategories { get; set; }

        [Column("prd_id")]
        public int IdProduits { get; set; }

        [ForeignKey(nameof(IdProduits))]
        public ICollection<Produit> Produits { get; set; }
    }
}
