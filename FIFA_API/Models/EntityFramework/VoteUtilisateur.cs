
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{

	[Table("t_e_voteutilisateur_vtl")]
    public class VoteUtilisateur
    {
        [Column("utl_id", Order = 0)]
        public int IdUtilisateur { get; set; }

        [Column("col_id", Order = 1)]
        public int IdCouleur { get; set; }

        [Column("tpr_id", Order = 2)]
        public int IdTaille { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur Utilisateur { get; set; }

        [ForeignKey(nameof(IdCouleur))]
        public Couleur Couleur { get; set; }

        [ForeignKey(nameof(IdTaille))]
        public TailleProduit Taille { get; set; }

        [Column("vtl_rankvote")]
        public int RankVote { get; set; }
    }
}
