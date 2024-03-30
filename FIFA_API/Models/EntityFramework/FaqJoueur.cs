using System.ComponentModel.DataAnnotations.Schema;
﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_faqjoueur_faq")]
    public partial class FaqJoueur
    {
        public const int MAX_QUESTION_LENGTH = 500;
        public const int MAX_REPONSE_LENGTH = 500;

        //Joueur
        [Column("jou_id"), Required]
        public int IdJoueur { get; set; }

        [ForeignKey(nameof(IdJoueur)), JsonIgnore]
        public Joueur Joueur { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("faq_id")]
        public int Id { get; set; }

        [Column("faq_question"), Required]
        [StringLength(MAX_QUESTION_LENGTH, MinimumLength = 1, ErrorMessage = "La question doit avoir entre 1 et 500 caractères.")]
        public string Question { get; set; } 

		[Column("faq_reponse"), Required]
        [StringLength(MAX_REPONSE_LENGTH, MinimumLength = 1, ErrorMessage = "La réponse doit avoir entre 1 et 500 caractères.")]
        public string Reponse { get; set; } 

    }
}
