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
            Variantes = new HashSet<VarianteCouleurProduit>();
            Tailles = new HashSet<TailleProduit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prd_id")]
        public int Id { get; set; }

        [Column("prd_titre"), Required]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères")]
        public string Titre { get; set; }

        [Column("prd_description"), Required]
        [StringLength(200, ErrorMessage = "La description ne doit pas dépasser 200 caractères")]
        public string Description { get; set; }

        [Column("cmp_id"), Required]
        public int IdCompetition { get; set; }

        [Column("nat_id"), Required]
        public int IdNation { get; set; }

        [Column("gen_id"), Required]
        public int IdGenre { get; set; }

        [Column("cpr_id"), Required]
        public int IdCategorieProduit { get; set; }

        [ForeignKey(nameof(IdCompetition))]
        public Competition Competition { get; set; }

        [ForeignKey(nameof(IdNation))]
        public Nation Nation { get; set; }

        [ForeignKey(nameof(IdGenre))]
        public Genre Genre { get; set; }

        [ForeignKey(nameof(IdCategorieProduit))]
        public CategorieProduit Categorie { get; set; }

        [InverseProperty(nameof(VarianteCouleurProduit.Produit))]
        public virtual ICollection<VarianteCouleurProduit> Variantes { get; set; }

        [ManyToMany(nameof(TailleProduit.Produits))]
        public virtual ICollection<TailleProduit> Tailles { get; set; }
    }
}
