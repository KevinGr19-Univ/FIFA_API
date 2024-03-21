using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_langue_lan")]
    public partial class Langue
    {
        public const int MAX_NOM_LENGTH = 50;

        public Langue()
        {
            Utilisateurs = new HashSet<Utilisateur>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("lan_id")]
        public int Id { get; set; }

        [Column("lan_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Utilisateur.Langue))]
        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
