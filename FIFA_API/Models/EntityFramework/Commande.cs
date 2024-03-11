using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_commande_cmd")]
	public partial class Commande
	{
        [Key]
        [Column("cmd_id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("tli_id")]
		public int IdTypeLivraison { get; set; }

		[Column("utl_id")]
		public int IdUtilisateur { get; set; }

		[Column("cmd_villelivraison")]
		[StringLength(100, ErrorMessage = "La ville de livraison ne doit pas d�passer 100 caract�res")]
		public string VilleLivraison { get; set; }

		[Column("cmd_ruelivraison")]
		[StringLength(100, ErrorMessage = "La rue de livraison ne doit pas d�passer 100 caract�res")]
		public string RueLivraison { get; set; }

		[Column("cmd_postallivraison", TypeName = "char")]
		[StringLength(5)]
		[RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage = "Le code postal de livraison doit �tre compos� de 5 caract�res, chiffres et lettres.")]
        public string PostalLivraison { get; set; }

		[Column("cmd_villefacturation")]
        [StringLength(100, ErrorMessage = "La ville de facturation ne doit pas d�passer 100 caract�res")]
        public string VilleFacturation { get; set; }

		[Column("cmd_ruefacturation")]
        [StringLength(100, ErrorMessage = "La rue de livraison ne doit pas d�passer 100 caract�res")]
        public string RueFacturation { get; set; }

		[Column("cmd_postalfacturation", TypeName = "char")]
        [StringLength(5)]
        [RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage = "Le code postal de facturation doit �tre compos� de 5 caract�res, chiffres et lettres.")]
        public string PostalFacturation { get; set; }

		[Column("cmd_prixlivraison")]
		public double PrixLivraison { get; set; }

		[Column("cmd_tva")]
        public double TVA { get; set; }
		
		[Column("cmd_dateexpedition")]
		public DateTime DateExpedition { get; set; }

		[Column("cmd_datelivraison")]
		public DateTime DateLivraison { get; set; }

		[Column("cmd_datecommande")]
		public DateTime DateCommande { get; set; }

		[Column("cmd_lienfacture")]
        [StringLength(500, ErrorMessage = "La ville de livraison ne doit pas d�passer 100 caract�res")]
        public string LienFacture { get; set; }
	}
}