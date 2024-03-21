using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_trophee_tph")]
    [Index(nameof(Nom), IsUnique = true)]
    public partial class Trophee
    {
        public const int MAX_NOM_LENGTH = 60;

        public Trophee()
        {
            Joueurs = new HashSet<Joueur>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tph_id")]
        public int Id { get; set; }

        [Column("tph_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 60 caractères.")]
        public string Nom { get; set; }

        public virtual ICollection<Joueur> Joueurs { get; set; }
    }
}
