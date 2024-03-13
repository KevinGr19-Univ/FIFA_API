using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_competition_cmp")]
    public class Competition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmp_id")]
        public int Id { get; set; }

        [Column("cmp_nom")]
        [StringLength(100, ErrorMessage = "Le nom de la compétition ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        public ICollection<Produit> Produits { get; set; }
    }
}
