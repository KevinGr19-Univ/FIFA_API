using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_auth2falogin_a2f")]
    public partial class Auth2FALogin
    {
        [Key, Column("utl_id")]
        public int IdUtilisateur { get; set; }

        [Column("a2f_date")]
        public DateTime Date { get; set; }

        [Column("a2f_code")]
        [StringLength(10)]
        public string Code { get; set; }

        [ForeignKey(nameof(IdUtilisateur)), JsonIgnore]
        [OnDelete(DeleteBehavior.Cascade)]
        public Utilisateur Utilisateur { get; set; }
    }
}
