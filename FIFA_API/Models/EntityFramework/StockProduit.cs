using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_stockproduit_spr")]
    public class StockProduit
    {
        [Key, Column("prd_id", Order = 0)]
        public int IdProduit { get; set; }

        [Key, Column("col_id", Order = 1)]
        public int IdCouleur { get; set; }

        [Key, Column("tpr_id", Order = 2)]
        public int IdTaille { get; set; }

        public VarianteCouleurProduit VCProduit { get; set; }

        [ForeignKey(nameof(IdTaille))]
        public TailleProduit Taille { get; set; }

		[Column("spr_stocks")]
        public int Stocks { get; set; }

    }
}
