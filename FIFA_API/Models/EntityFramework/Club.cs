using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_club_clb")]
    [Index(nameof(Nom),IsUnique =true)]
    public partial class Club
    {
        public const int MAX_NOM_LENGTH = 100;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clb_id")]
        public int Id { get; set; }

        [Column("clb_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom du club ne doit pas dépasser les 100 caractères")]
        public string Nom { get; set; }


    }
}
