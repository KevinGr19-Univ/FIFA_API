using FIFA_API.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_genre_gen")]
    public partial class Genre : IVisible
    {
        public Genre()
        {
            Produits = new HashSet<Produit>();
        }

        public const int MAX_NOM_GENRE = 50;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("gen_id")]
        public int Id { get; set; }

        [Column("gen_nom"), Required]
        [StringLength(MAX_NOM_GENRE, ErrorMessage = "Le nom du genre ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Produit.Genre))]
        public ICollection<Produit> Produits { get; set; }

        [Column("gen_visible")]
        public bool Visible { get; set; } = true;
    }
}
