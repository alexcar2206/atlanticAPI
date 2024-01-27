using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class CategoriaArchivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoriaArchivo {  get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

    }
}
