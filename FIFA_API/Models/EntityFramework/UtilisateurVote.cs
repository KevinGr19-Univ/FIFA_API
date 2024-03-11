using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_utilisateur_uvt")]
	public partial class UtilisateurVote
	{
		[Key]
		[Column("uvt_idutilisateur")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUtilisateur;

        [Key]
        [Column("uvt_idthemevote")]
        public int IdThemeVote;

        [Key]
        [Column("uvt_idjoueur")]
        public int IdJoueur;

		[Column("uvt_rank")]
		public int Rank;
	}
}