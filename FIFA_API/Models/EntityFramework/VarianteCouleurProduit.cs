using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_variantecouleurproduit_vcp")]
    [Index(nameof(IdProduit), nameof(IdCouleur), IsUnique = true)]
    public partial class VarianteCouleurProduit
    {
        public VarianteCouleurProduit() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("vcp_id")]
        public int Id { get; set; }

        [Column("prd_id", Order = 0), Required]
        public int IdProduit { get; set; }

        [Column("col_id", Order = 1), Required]
        public int IdCouleur { get; set; }

		[Column("vcp_prix"), Required]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        [Precision(7,2)]
        public decimal Prix { get; set; }

        [Column("vcp_images", TypeName = "varchar[]")]
        public List<string> ImageUrls { get; set; } = new List<string>();

        [ForeignKey(nameof(IdProduit)), JsonIgnore]
        public virtual Produit Produit { get; set; }

        [ForeignKey(nameof(IdCouleur)), JsonIgnore]
        public virtual Couleur Couleur { get; set; }

        [InverseProperty(nameof(StockProduit.VCProduit))]
        public virtual ICollection<StockProduit> Stocks { get; set; }
    }
}
