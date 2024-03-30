using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_lignecommande_lco")]
    public partial class LigneCommande
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("lco_id")]
        public int Id { get; set; }

        [Column("lco_quantite"), Required]
        public int Quantite { get; set; }

		[Column("lco_prixunitaire"), Required]
        [Precision(7,2)]
        public decimal PrixUnitaire { get; set; }

        //VarianteCouleurProduit
        [Column("prd_id"), Required]
        public int IdVCProduit { get; set; }

        [OnDelete(DeleteBehavior.SetNull)]
        [ForeignKey(nameof(IdVCProduit))]
        public VarianteCouleurProduit VCProduit { get; set; }

        //Taille
        [Column("tpr_id"), Required]
        public int IdTaille { get; set; }

        [OnDelete(DeleteBehavior.SetNull)]
        [ForeignKey(nameof(IdTaille)), JsonIgnore]
        public TailleProduit Taille { get; set; }

        //Commande
        [Column("cmd_id"), Required]
        public int IdCommande { get; set; }

        [OnDelete(DeleteBehavior.Cascade)]
        [ForeignKey(nameof(IdCommande)), JsonIgnore]
        public Commande Commande { get; set; }
    }
}
