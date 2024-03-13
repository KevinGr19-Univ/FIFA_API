using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_document_doc")]
    public class Document : Publication
    {
        public const int MAX_URLPDF_LENGTH = 500;

		[Column("doc_urlpdf")]
        [StringLength(MAX_URLPDF_LENGTH, ErrorMessage = "L'URL du PDF ne doit pas dépasser 500 caractères.")]
        public string UrlPdf { get; set; }

    }
}
