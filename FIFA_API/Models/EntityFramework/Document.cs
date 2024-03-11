using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_document_doc")]
    public partial class Document
	{
        [Key]
        [Column("doc_id")]
        public int Id { get; set; }

        //Foreign Key
        [Column("doc_idphoto")]
        public int IdPhoto { get; set; }

        [Column("doc_titre")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères")]
        public string TitrePublication { get; set; } = null!;
        [Column("doc_resume")]
        [StringLength(500, ErrorMessage = "Le résumé ne doit pas dépasser 500 caractères")]
        public string ResumePublication { get; set; } = null!;

        [Column("doc_date")]
        public DateTime? DatePublication { get; set; }

        [Column("doc_urlpdf")]
        [StringLength(500, ErrorMessage = "L'URL du pdf ne doit pas dépasser 500 caractères")]
        public string UrlPdf { get; set; } = null!;
    }
}