using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_lignecommande_lco")]
    public class LigneCommande
    {
        public VarianteCouleurProduit VCProduit { get; set; }
        public TailleProduit Taille { get; set; }
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
