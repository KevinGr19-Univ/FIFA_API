using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_h_blog_blg")]
    public class Blog : Publication
    {
		[Column("blg_texte")]
        public string Texte { get; set; }

        [Column("pht_id")]
        public int IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        public ICollection<Photo> Photos { get; set; }
    }
}
