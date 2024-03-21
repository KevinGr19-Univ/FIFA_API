﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_themevote_thv")]
    [Index(nameof(NomTheme), IsUnique = true)]
    public class ThemeVote
    {
        public const int MAX_NOMTHEME_LENGTH = 60;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("thv_id")]
        public int Id { get; set; }

        [Column("thv_nomtheme"), Required]
        [StringLength(MAX_NOMTHEME_LENGTH, ErrorMessage = "Le nom du thème ne doit pas dépasser 60 caractères")]
        public string NomTheme { get; set; }
    }
}