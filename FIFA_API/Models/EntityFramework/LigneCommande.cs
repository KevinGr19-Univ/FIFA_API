using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_lignecommande_lco")]
    public class LigneCommande
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("lco_id")]
        public int Id { get; set; }

        [Column("lco_quantite")]
        public int Quantite { get; set; }

		[Column("lco_prixunitaire")]
        [Precision(7,2)]
        public decimal PrixUnitaire { get; set; }

        //VarianteCouleurProduit
        [Column("prd_id")]
        public int IdVCProduit { get; set; }

        [ForeignKey(nameof(IdVCProduit))]
        public VarianteCouleurProduit VCProduit { get; set; }

        //Taille
        [Column("tpr_id")]
        public int IdTaille { get; set; }

        [ForeignKey(nameof(IdTaille))]
        public TailleProduit Taille { get; set; }

        //Commande
        [Column("cmd_id")]
        public int IdCommande { get; set; }

        [ForeignKey(nameof(IdCommande))]
        public Commande Commande { get; set; }
    }
}
