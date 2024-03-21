using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_nation_nat")]
    public partial class Nation
    {
        public const int MAX_NOM_LENGTH = 50;

        public Nation()
        {
            Produits = new HashSet<Produit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("nat_id")]
        public int Id { get; set; }

        [Column("nat_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Produit.Nation))]
        public ICollection<Produit> Produits { get; set; }
    }
}
