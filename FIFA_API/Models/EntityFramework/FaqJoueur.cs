using System.ComponentModel.DataAnnotations.Schema;
﻿using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_faqjoueur_faq")]
    public class FaqJoueur
    {
        public const int MAX_QUESTION_LENGTH = 500;
        public const int MAX_REPONSE_LENGTH = 500;

        //Joueur
        [Column("jou_id")]
        public int IdJoueur { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        public Joueur Joueur { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("faq_id")]
        public int Id { get; set; }

        [Column("faq_question")]
        [StringLength(MAX_QUESTION_LENGTH, MinimumLength = 1, ErrorMessage = "La question doit avoir entre 1 et 500 caractères.")]
        public string Question { get; set; } 

		[Column("faq_reponse")]
        [StringLength(MAX_REPONSE_LENGTH, MinimumLength = 1, ErrorMessage = "La réponse doit avoir entre 1 et 500 caractères.")]
        public string Reponse { get; set; } 

    }
}
