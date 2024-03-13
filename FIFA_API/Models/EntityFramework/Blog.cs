using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_h_blog_blg")]
    public class Blog : Publication
    {
        public Blog()
        {
            Photos = new HashSet<Photo>();
        }

        [Column("blg_texte", TypeName ="text")]
        public string Texte { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
