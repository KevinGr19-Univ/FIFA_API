using FIFA_API.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_competition_cmp")]
    public partial class Competition : IVisible
    {
        public const int MAX_NOM_LENGTH = 100;

        public Competition()
        {
            Produits = new HashSet<Produit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmp_id")]
        public int Id { get; set; }

        [Column("cmp_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom de la compétition ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Produit.Competition))]
        public ICollection<Produit> Produits { get; set; }

        [Column("cmp_visible")]
        public bool Visible { get; set; } = true;
    }
}
