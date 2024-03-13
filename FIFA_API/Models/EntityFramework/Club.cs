using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_club_clb")]
    [Index(nameof(Nom),IsUnique =true)]
    public class Club
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clb_id")]
        public int Id { get; set; }

        [Column("clb_nom")]
        [StringLength(100, ErrorMessage = "Le nom du club ne doit pas dépasser les 100 caractères")]
        public string Nom { get; set; }


    }
}
