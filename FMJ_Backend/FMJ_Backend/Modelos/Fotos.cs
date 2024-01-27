using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class Fotos
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Foto { get; set; }

        //Puede ser de tipo: PERFIL, DNI, PUBLICACION  
        public string Tipo { get; set; }
        public string? Foto { get; set; }
        public string? contentType { get; set; }

        [ForeignKey("Usuarias")]
        public int Fk_idUsuaria { get; set; }

    }
}
