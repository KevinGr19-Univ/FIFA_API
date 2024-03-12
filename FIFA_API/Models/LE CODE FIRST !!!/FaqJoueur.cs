using System.ComponentModel.DataAnnotations.Schema;
﻿using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_faqjoueur_faq")]
    public class FaqJoueur
    {

        public Joueur Joueur { get; set; }

		[Column("faq_question")]
        public string Question { get; set; } 


		[Column("faq_reponse")]
        public string Reponse { get; set; } 

    }
}
