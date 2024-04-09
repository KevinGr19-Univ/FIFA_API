using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_stockproduit_spr")]
    [ComposedKey(nameof(IdVCProduit), nameof(IdTaille))]
    public partial class StockProduit
    {
        [Column("vcp_id", Order = 0), Required]
        public int IdVCProduit { get; set; }

        [Column("tpr_id", Order = 1), Required]
        public int IdTaille { get; set; }

        [ForeignKey(nameof(IdVCProduit)), JsonIgnore]
        public VarianteCouleurProduit VCProduit { get; set; }

        [ForeignKey(nameof(IdTaille)), JsonIgnore]
        public TailleProduit Taille { get; set; }

		[Column("spr_stocks")]
        [Range(0, int.MaxValue, ErrorMessage = "Les stocks ne peuvent pas être négatifs")]
        public int Stocks { get; set; }

    }
}
