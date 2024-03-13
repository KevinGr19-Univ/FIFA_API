using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_nation_nat")]
    public class Nation
    {
        [Key]
        [Column("nat_id")]
        public int Id { get; set; }

        [Column("nat_nom")]
        public string Nom { get; set; }


        public ICollection<Produit> Produits { get; set; }
    }
}
