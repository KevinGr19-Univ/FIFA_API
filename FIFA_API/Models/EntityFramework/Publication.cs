using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_g_publication_pub")]
    public abstract partial class Publication
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pub_id")]
        public int Id { get; set; }

        [Column("pub_titre")]
        public string Titre { get; set; }

		[Column("pub_resume")]
        public string Resume { get; set; }

		[Column("pub_datepublication")]
        public DateTime DatePublication { get; set; }

        [Column("pht_id")]
        public int? IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        public Photo? Photo { get; set; }
    }
}
