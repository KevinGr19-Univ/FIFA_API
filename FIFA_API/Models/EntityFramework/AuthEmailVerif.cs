using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_authemailverif_aev")]
    public class AuthEmailVerif
    {
        [Key, Column("utl_id")]
        public int IdUtilisateur { get; set; }

        [Column("utl_mail")]
        public string Mail { get; set; }

        [Column("aev_code")]
        public string Code { get; set; }

        [Column("aev_date")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(IdUtilisateur)), JsonIgnore]
        [OnDelete(DeleteBehavior.Cascade)]
        public Utilisateur Utilisateur { get; set; }
    }
}
