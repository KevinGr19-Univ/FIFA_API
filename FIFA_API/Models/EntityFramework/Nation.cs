using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_nation_nat")]
    public partial class Nation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("nat_id")]
        public int Id { get; set; }

        [Column("nat_nom")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Produit.Nation))]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
