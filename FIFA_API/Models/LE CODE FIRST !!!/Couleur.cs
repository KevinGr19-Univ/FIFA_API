using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_couleur_col")]
    public class Couleur
    {
        [Key]
        [Column("clb_id")]
        public int Id { get; set; }

        [Column("col_nom")]
        public string Nom { get; set; }

		[Column("col_codehexa")]
        public string CodeHexa { get; set; }

    }
}
