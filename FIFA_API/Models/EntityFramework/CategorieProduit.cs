using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_categorieproduit_cpr")]
    [Index(nameof(Nom), IsUnique = true)]
    public class CategorieProduit
    {
        public CategorieProduit()
        {
            SousCategories = new HashSet<CategorieProduit>();
            Produits = new HashSet<Produit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cpr_id")]
        public int Id { get; set; }

        [Column("cpr_nom")]
        [StringLength(100, ErrorMessage = "Le nom de la catégorie ne doit pas dépasser les 100 caractères")]
        public string Nom { get; set; }

        [Column("cpr_id2")]
        public int IdCategorieProduitParent { get; set; }

        [ForeignKey(nameof(IdCategorieProduitParent))]
        public CategorieProduit? Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        public ICollection<CategorieProduit> SousCategories { get; set; }

        [InverseProperty(nameof(Produit.Categorie))]
        public ICollection<Produit> Produits { get; set; }
    }
}
