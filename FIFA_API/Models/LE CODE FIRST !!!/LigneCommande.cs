using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_lignecommande_lco")]
    public class LigneCommande
    {

        //VarianteCouleurProduit

        [Column("vcp_id")]
        public int IdVCProduit {  get; set; }

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





        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("lco_id")]
        public int Id { get; set; }

        [Column("lco_quantite")]
        public int Quantite { get; set; }

		[Column("lco_prixunitaire")]
        public decimal PrixUnitaire { get; set; }

    }
}
