using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_document_doc")]
    public class Document : Publication
    {
		[Column("doc_urlpdf")]
        public string UrlPdf { get; set; }

    }
}
