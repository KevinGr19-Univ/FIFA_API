using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_article_art")]
    public class Article : Publication
    {
		[Column("art_texte", TypeName = "text")]
        public string Texte { get; set; }

    }
}
