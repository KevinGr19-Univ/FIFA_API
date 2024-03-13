using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_competition_cmp")]
    public class Competition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmp_id")]
        public int Id { get; set; }

        [Column("cmp_nom")]
        public string Nom { get; set; }

        [Column("prd_id")]
        public int IdProduit { get; set; }

        [ForeignKey(nameof(IdProduit))]
        public ICollection<Produit> Produits { get; set; }
    }
}
