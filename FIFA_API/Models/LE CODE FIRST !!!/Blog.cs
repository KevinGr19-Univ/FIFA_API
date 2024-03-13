using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_blog_blg")]
    public class Blog : Publication
    {
		[Column("blg_texte")]
        public string Texte { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
