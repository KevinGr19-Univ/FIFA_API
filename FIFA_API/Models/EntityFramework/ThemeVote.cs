using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_themevote_thv")]
    public class ThemeVote
    {
        public const int MAX_NOMTHEME_LENGTH = 60;

        [Key, Column("thv_nomtheme")]
        [StringLength(MAX_NOMTHEME_LENGTH, ErrorMessage = "Le nom du thème ne doit pas dépasser 60 caractères")]
        public string NomTheme { get; set; }
    }
}
