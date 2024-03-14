using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_produit_prd")]
    public partial class Produit
    {
        public Produit()
        {
            Associes = new HashSet<Produit>();
            Couleurs = new HashSet<Couleur>();
            Tailles = new HashSet<TailleProduit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prd_id")]
        public int Id { get; set; }

		[Column("prd_titre")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas d�passer 100 caract�res")]
        public string Titre { get; set; }

		[Column("prd_description")]
        [StringLength(200, ErrorMessage = "La description ne doit pas d�passer 200 caract�res")]
        public string Description { get; set; }

        [Column("cmp_id")]
        public int IdCompetition { get; set; }

        [Column("nat_id")]
        public int IdNation { get; set; }

        [Column("gen_id")]
        public int IdGenre { get; set; }

        [Column("cpr_id")]
        public int IdCategorieProduit { get; set; }

        [ForeignKey(nameof(IdCompetition))]
        public Competition Competition { get; set; }

        [ForeignKey(nameof(IdNation))]
        public Nation Nation { get; set; }

        [ForeignKey(nameof(IdGenre))]
        public Genre Genre { get; set; }

        [ForeignKey(nameof(IdCategorieProduit))]
        public CategorieProduit Categorie { get; set; }

        public virtual ICollection<Produit> Associes { get; set; }
        public virtual ICollection<Produit> AssociesTo { get; set; }

        public virtual ICollection<Couleur> Couleurs { get; set; }
        public virtual ICollection<TailleProduit> Tailles { get; set; }
    }
}
