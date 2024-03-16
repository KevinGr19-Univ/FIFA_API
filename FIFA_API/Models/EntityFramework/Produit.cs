using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_produit_prd")]
    public partial class Produit
    {
        public Produit()
        {
            Couleurs = new HashSet<Couleur>();
            Tailles = new HashSet<TailleProduit>();

            Associes = new HashSet<ProduitProduit>();
            AssociesTo = new HashSet<ProduitProduit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prd_id")]
        public int Id { get; set; }

		[Column("prd_titre")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères")]
        public string Titre { get; set; }

		[Column("prd_description")]
        [StringLength(200, ErrorMessage = "La description ne doit pas dépasser 200 caractères")]
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

        [InverseProperty(nameof(ProduitProduit.Produit1))]
        public virtual ICollection<ProduitProduit> Associes { get; set; }

        [InverseProperty(nameof(ProduitProduit.Produit2))]
        public virtual ICollection<ProduitProduit> AssociesTo { get; set; }

        [ManyToMany(nameof(Couleur.Produits))]
        public virtual ICollection<Couleur> Couleurs { get; set; }

        [ManyToMany(nameof(TailleProduit.Produits))]
        public virtual ICollection<TailleProduit> Tailles { get; set; }
    }
}
