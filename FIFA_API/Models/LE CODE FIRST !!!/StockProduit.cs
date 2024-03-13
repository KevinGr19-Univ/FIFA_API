using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_j_stockproduit_spr")]
    public class StockProduit
    {
        [Key]
        [Column("spr_id")]
        public int Id { get; set; }
        public VarianteCouleurProduit VCProduit { get; set; }
        public TailleProduit Taille { get; set; }

		[Column("spr_stocks")]
        public int Stocks { get; set; }

    }
}
