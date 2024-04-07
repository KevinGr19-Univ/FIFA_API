using FIFA_API.Models.Annotations;
using FIFA_API.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_produit_prd")]
    public partial class Produit : IVisible
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prd_id")]
        public int Id { get; set; }

        [Column("prd_titre"), Required]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères")]
        public string Titre { get; set; }

        [Column("prd_description"), Required]
        public string Description { get; set; }

        [Column("cmp_id")]
        public int? IdCompetition { get; set; }

        [Column("nat_id")]
        public int? IdNation { get; set; }

        [Column("gen_id")]
        public int? IdGenre { get; set; }

        [Column("cpr_id"), Required]
        public int IdCategorieProduit { get; set; }

        [ForeignKey(nameof(IdCompetition))]
        public Competition? Competition { get; set; }

        [ForeignKey(nameof(IdNation))]
        public Nation? Nation { get; set; }

        [ForeignKey(nameof(IdGenre))]
        public Genre? Genre { get; set; }

        [ForeignKey(nameof(IdCategorieProduit))]
        public CategorieProduit Categorie { get; set; }

        [InverseProperty(nameof(VarianteCouleurProduit.Produit))]
        public ICollection<VarianteCouleurProduit> Variantes { get; set; } = new HashSet<VarianteCouleurProduit>();

        [ManyToMany(nameof(TailleProduit.Produits))]
        public ICollection<TailleProduit> Tailles { get; set; } = new HashSet<TailleProduit>();

        [Column("prd_visible")]
        public bool Visible { get; set; } = true;
    }
}
