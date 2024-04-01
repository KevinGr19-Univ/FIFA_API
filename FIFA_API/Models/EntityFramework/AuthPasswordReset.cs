using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_authpasswordreset_apr")]
    public class AuthPasswordReset
    {
        [Key, Column("utl_mail")]
        public string Mail { get; set; }

        [Column("apr_date")]
        public DateTime Date { get; set; }

        [Column("apr_code")]
        public string Code { get; set; }
    }
}
