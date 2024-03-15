using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_j_produitassocies_pas")]
    [ComposedKey(nameof(Id1), nameof(Id2))]
    public partial class ProduitProduit
    {
        [Column("prd_id1")]
        public int Id1 { get; set; }

        [Column("prd_id2")]
        public int Id2 { get; set; }

        [ForeignKey(nameof(Id1))]
        [OnDelete(DeleteBehavior.Cascade)]
        public Produit Produit1 { get; set; }

        [ForeignKey(nameof(Id2))]
        [OnDelete(DeleteBehavior.Cascade)]
        public Produit Produit2 { get; set; }
    }
}
