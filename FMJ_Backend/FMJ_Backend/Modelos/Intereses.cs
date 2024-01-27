using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FMJ_Backend.Modelos
{
    public class Intereses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_interes { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Icon { get; set; }
    }
}
