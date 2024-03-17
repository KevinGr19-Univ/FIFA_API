using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_photo_pht")]
    public partial class Photo
    {
        public const int MAX_NOM_LENGTH = 50;
        public const int MAX_URL_LENGTH = 500;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pht_id")]
        public int Id { get; set; }

        [Column("pht_nom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

		[Column("pht_url")]
        [StringLength(MAX_URL_LENGTH, ErrorMessage = "L'url ne doit pas dépasser 500 caractères")]
        public string Url { get; set; }

        #region Many-to-many
        private ICollection<Album> _albums { get; set; }
        private ICollection<Article> _articles { get; set; }
        private ICollection<Blog> _blogs { get; set; }
        private ICollection<Joueur> _joueurs { get; set; }
        #endregion
    }
}
