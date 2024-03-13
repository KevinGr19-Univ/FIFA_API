using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_document_doc")]
    public class Document : Publication
    {
		[Column("doc_urlpdf")]
        public string UrlPdf { get; set; }

    }
}
