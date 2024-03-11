using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_uvt")]
	public partial class UtilisateurVote
	{
		[Key]
		[Column("uvt_idutilisateur")]
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