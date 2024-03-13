using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_adresse_adr")]
    public class Adresse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adr_id")]
        public int Id { get; set; }

		[Column("adr_ville")]
        [StringLength(100, ErrorMessage = "Le nom de la ville ne doit pas dépasser les 100 caractères")]
        public string Ville { get; set; }

		[Column("adr_rue")]
        [StringLength(100, ErrorMessage = "Le nom de la rue ne doit pas dépasser les 100 caractères")]
        public string Rue { get; set; }

		[Column("adr_codepostal", TypeName = "char")]
        [StringLength(5, MinimumLength = 5)]
        [RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage ="Votre code postal ne respecte pas les normes françaises")]
        public string CodePostal { get; set; }

    }
}

