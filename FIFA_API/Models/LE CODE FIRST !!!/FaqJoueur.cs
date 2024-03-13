using System.ComponentModel.DataAnnotations.Schema;
﻿using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_faqjoueur_faq")]
    public class FaqJoueur
    {

        //Joueur
        [Column("jou_id")]
        public int IdJoueur { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        public Joueur Joueur { get; set; }





        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("faq_id")]
        public int Id { get; set; }

        [Column("faq_question")]
        public string Question { get; set; } 


		[Column("faq_reponse")]
        public string Reponse { get; set; } 

    }
}
