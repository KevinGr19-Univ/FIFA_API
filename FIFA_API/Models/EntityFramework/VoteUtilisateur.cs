
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{

	[Table("t_e_voteutilisateur_vtl")]
    public class VoteUtilisateur
    {
        [Key, Column("utl_id", Order = 0)]
        public int IdUtilisateur { get; set; }

        [Key, Column("col_id", Order = 1)]
        public int IdCouleur { get; set; }

        [Key, Column("tpr_id", Order = 2)]
        public int IdTaille { get; set; }

        [Column("vtl_rankvote")]
        public int RankVote { get; set; }
    }
}
