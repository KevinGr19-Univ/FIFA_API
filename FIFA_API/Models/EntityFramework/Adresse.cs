using FIFA_API.Models.Utils;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_adresse_adr")]
    public class Adresse
    {
        public const int MAX_VILLE_LENGTH = 100;
        public const int MAX_RUE_LENGTH = 200;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adr_id")]
        public int Id { get; set; }

        [Column("adr_ville"), Required]
        [StringLength(MAX_VILLE_LENGTH, ErrorMessage = "Le nom de la ville ne doit pas dépasser les 100 caractères")]
        public string Ville { get; set; }

		[Column("adr_rue"), Required]
        [StringLength(MAX_RUE_LENGTH, ErrorMessage = "Le nom de la rue ne doit pas dépasser les 200 caractères")]
        public string Rue { get; set; }

		[Column("adr_codepostal"), Required]
        [StringLength(5, MinimumLength = 5)]
        [RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage ="Votre code postal ne respecte pas les normes françaises")]
        public string CodePostal { get; set; }

    }
}

