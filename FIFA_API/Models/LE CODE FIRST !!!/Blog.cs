using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_blog_blg")]
    public class Blog : Publication
    {
		[Column("blg_texteblog")]
        public string TexteBlog { get; set; }


        public ICollection<Photo> Photos { get; set; }
    }
}
