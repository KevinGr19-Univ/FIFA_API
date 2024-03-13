using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_g_publication_pub")]
    public abstract class Publication
    {
        [Key]
        [Column("pub_id")]
        public int Id { get; set; }

        [Column("pub_titre")]
        public string Titre { get; set; }

		[Column("pub_resume")]
        public string Resume { get; set; }

		[Column("pub_datepublication")]
        public DateTime DatePublication { get; set; }

        public Photo? Photo { get; set; }
    }
}
