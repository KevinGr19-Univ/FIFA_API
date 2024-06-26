using FIFA_API.Models.Annotations;
using FIFA_API.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_g_publication_pub")]
    public abstract partial class Publication : IVisible
    {
        public const int MAX_TITRE_LENGTH = 200;
        public const int MAX_RESUME_LENGTH = 600;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pub_id")]
        public int Id { get; set; }

        [Column("pub_titre"), Required]
        [StringLength(MAX_TITRE_LENGTH, ErrorMessage = "Le titre ne doit pas dépasser 200 caractères.")]
        public string Titre { get; set; }

		[Column("pub_resume"), Required]
        [StringLength(MAX_RESUME_LENGTH, ErrorMessage = "Le résumé ne doit pas dépasser 600 caractères.")]
        public string Resume { get; set; }

		[Column("pub_datepublication")]
        public DateTime DatePublication { get; set; }

        [Column("pht_id")]
        public int? IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        [OnDelete(DeleteBehavior.SetNull)]
        public Photo? Photo { get; set; }

        [JsonIgnore]
        public ICollection<Joueur> Joueurs { get; set; }

        [Column("pub_visible")]
        public bool Visible { get; set; } = true;
    }
}
