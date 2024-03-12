using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_article_art")]
    public class Article : Publication
    {
		[Column("art_texte")]
        public string Texte { get; set; }

    }
}
