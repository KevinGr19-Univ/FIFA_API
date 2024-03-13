using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_couleur_col")]
    public class Couleur
    {
        public const int MAX_NOM_LENGTH = 50;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("col_id")]
        public int Id { get; set; }

        [Column("col_nom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom de la couleur ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; }


		[Column("col_codehexa")]
        [RegularExpression(ModelUtils.REGEX_HEXACOLOR, ErrorMessage = "Le code hexadécimal de couleur doit être au format hexadécimal.")]
        public string CodeHexa { get; set; }

    }
}
