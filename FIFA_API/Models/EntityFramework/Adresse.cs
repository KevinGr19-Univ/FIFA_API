using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_adresse_adr")]
    public class Adresse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adr_id")]
        public int Id { get; set; }

		[Column("adr_ville")]
        public string Ville { get; set; }

		[Column("adr_rue")]
        public string Rue { get; set; }

		[Column("adr_codepostal")]
        public string CodePostal { get; set; }

    }
}

