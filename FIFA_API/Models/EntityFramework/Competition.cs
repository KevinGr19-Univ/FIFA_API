using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_competition_cmp")]
    public partial class Competition
    {
        public const int MAX_NOM_LENGTH = 100;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmp_id")]
        public int Id { get; set; }

        [Column("cmp_nom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom de la compétition ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Produit.Competition))]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
