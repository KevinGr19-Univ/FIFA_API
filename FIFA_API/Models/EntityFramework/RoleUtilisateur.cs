using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_r_roleutilisateur_rut")]
    public partial class RoleUtilisateur
    {
        [Column("rut_id")]
        public int Id { get; set; }

        [InverseProperty(nameof(Utilisateur.Role))]
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
