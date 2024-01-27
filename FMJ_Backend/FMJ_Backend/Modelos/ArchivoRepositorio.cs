using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class ArchivoRepositorio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdArchivoRepositorio {  get; set; }
        public string ArchivoBlob { get; set; }
        public string ContentType { get; set; }
    }
}
