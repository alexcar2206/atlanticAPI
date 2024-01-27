using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_rol { get; set; }
        public string Nombre { get; set; }
        public string Accesos { get; set; }
        public string Descripcion { get; set; }
    }
}
