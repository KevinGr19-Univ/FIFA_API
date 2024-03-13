using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_document_doc")]
    public class Document : Publication
    {
		[Column("doc_urlpdf")]
        [StringLength(500, ErrorMessage = "L'URL du PDF ne doit pas dépasser 500 caractères.")]
        public string UrlPdf { get; set; }

    }
}
