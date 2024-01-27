using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FMJ_Backend.Modelos
{
    public class Insignias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_insignia { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Icon { get; set; }
    }
}
