using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_pays_pys")]
    public partial class Pays
    {
        public const int MAX_NOM_LENGTH = 50;

        public Pays()
        {
            Utilisateurs = new HashSet<Utilisateur>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pys_id")]
        public int Id { get; set; }

        [Column("pys_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [InverseProperty(nameof(Utilisateur.Pays))]
        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
