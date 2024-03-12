using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_article_art")]
    public class Article : Publication
    {
		[Column("art_textearticle")]
        public string TexteArticle { get; set; }

    }
}
