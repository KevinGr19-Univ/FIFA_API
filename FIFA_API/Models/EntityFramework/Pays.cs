using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_pays_pys")]
    public class Pays
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pys_id")]
        public int Id { get; set; }

        [Column("pys_nom")]
        [StringLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
