using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_typelivraison_tli")]
    public partial class TypeLivraison
	{
		[Key]
		[Column("tli_id")]
		public int Id { get; set; }

		[Column("tli_nom")]
        [StringLength(50)]
		public string Nom { get; set; }

        [Column("tli_prix")]
        [Precision(2)]
        public double Prix { get; set; }

        [Column("tli_maxbusinessdays")]
        public int MaxBusinessDays { get; set; }
    }
}