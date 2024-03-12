using System.ComponentModel.DataAnnotations.Schema;

﻿using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_produit_prd")]
    public class Produit
    {
        public Competition Competition { get; set; }
        public Nation Nation { get; set; }
        public Genre Genre { get; set; }

		[Column("prd_titre")]
        public string Titre { get; set; }

		[Column("prd_description")]
        public string Description { get; set; }


        public ICollection<Produit> Associes { get; set; }
        public ICollection<Couleur> Couleurs { get; set; }
        public ICollection<TailleProduit> Tailles { get; set; }
    }
}
