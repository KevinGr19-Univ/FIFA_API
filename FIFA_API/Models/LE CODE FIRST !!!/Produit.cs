using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_produit_prd")]
    public class Produit
    {
        [Key]
        [Column("prd_id")]
        public int Id { get; set; }

		[Column("prd_titre")]
        public string Titre { get; set; }

		[Column("prd_description")]
        public string Description { get; set; }

        [Column("cmp_id")]
        public int IdCompetition { get; set; }

        [Column("nat_id")]
        public int IdNation { get; set; }

        [Column("gen_id")]
        public int IdGenre { get; set; }

        [ForeignKey(nameof(IdCompetition))]
        public Competition Competition { get; set; }

        [ForeignKey(nameof(IdNation))]
        public Nation Nation { get; set; }

        [ForeignKey(nameof(IdGenre))]
        public Genre Genre { get; set; }

        public ICollection<Produit> Associes { get; set; }
        public ICollection<Couleur> Couleurs { get; set; }
        public ICollection<TailleProduit> Tailles { get; set; }
    }
}
