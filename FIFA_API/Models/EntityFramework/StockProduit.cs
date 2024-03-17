using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_stockproduit_spr")]
    [ComposedKey(nameof(IdVCProduit), nameof(IdTaille))]
    public partial class StockProduit
    {
        [Column("vcp_id", Order = 0)]
        public int IdVCProduit { get; set; }

        [Column("tpr_id", Order = 1)]
        public int IdTaille { get; set; }

        [ForeignKey(nameof(IdVCProduit))]
        public VarianteCouleurProduit VCProduit { get; set; }

        [ForeignKey(nameof(IdTaille))]
        public TailleProduit Taille { get; set; }

		[Column("spr_stocks")]
        public int Stocks { get; set; }

    }
}
