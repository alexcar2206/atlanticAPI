using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class Repositorio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRepositorio { get; set; }
        public string? TituloArchivo { get; set; }

        [ForeignKey("Usuarias")]
        public int IdUsuaria { get; set; }
        public string? Descripcion { get; set; }

        [ForeignKey("ArchivoRepositorio")]
        public int IdArchivoRepositorio { get; set; }

        [ForeignKey("Usuarias")]
        public int DirigidoA {  get; set; }
        public string Estado { get; set; }
        public DateTime Registration_Date { get; set; }

        [ForeignKey("CategoriaArchivo")]
        public int IdCategoriaArchivo { get; set; }
        public string? FileName { get; set; }
    }
}
