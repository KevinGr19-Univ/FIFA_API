using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_trophee_trp")]
    public class Trophee
    {
        [Column("trp_id")]
        [Key()]
        public int Id { get; set; }


        [Column("trp_nom")]
        [StringLength(100, ErrorMessage = "Le nom du trophée ne doit pas dépasser les 100 caractères")]
        public String Nom { get; set; }
    }
}
