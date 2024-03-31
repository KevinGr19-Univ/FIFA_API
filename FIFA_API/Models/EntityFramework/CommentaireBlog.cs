using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_j_commentaireblog_cbl")]
    public partial class CommentaireBlog
    {
        public const int MAX_TEXTE_LENGTH = 500;

        private readonly ILazyLoader _loader;

        public CommentaireBlog() { }
        public CommentaireBlog(ILazyLoader loader)
        {
            _loader = loader;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cbl_id")]
        public int Id { get; set; }

        [Column("pub_id")]
        public int IdBlog { get; set; }

        [Column("utl_id")]
        public int? IdUtilisateur { get; set; }

        [Column("cbl_id_original")]
        public int? IdOriginal { get; set; }

        [Column("cbl_estreponse")]
        public bool EstReponse { get; set; } = false;

        [Column("cbl_date")]
        public DateTime Date { get; set; }

        [Column("cbl_texte"), Required]
        [StringLength(MAX_TEXTE_LENGTH, ErrorMessage = "Le texte d'un commentaire ne doit pas dépasser 500 caractères.")]
        public string Texte { get; set; }

        private Utilisateur? _utilisateur;

        [ForeignKey(nameof(IdBlog)), JsonIgnore]
        [OnDelete(DeleteBehavior.Cascade)]
        public Blog Blog { get; set; }

        [ForeignKey(nameof(IdUtilisateur)), JsonIgnore]
        [OnDelete(DeleteBehavior.SetNull)]
        public Utilisateur? Utilisateur
        {
            get => _loader.Load(this, ref _utilisateur);
            set => _utilisateur = value;
        }

        [ForeignKey(nameof(IdOriginal)), JsonIgnore]
        [OnDelete(DeleteBehavior.SetNull)]
        public CommentaireBlog? CommentaireOriginal { get; set; }

        public string Auteur => IdUtilisateur is null ?
            "<< Supprimé >>" :
            Utilisateur?.Surnom ?? "<< Anonyme >>";
    }
}
