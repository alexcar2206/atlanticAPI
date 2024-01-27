using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class InteresesUsuarias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_InteresesUsuarias { get; set; }

        [ForeignKey("Intereses")]
        public int Fk_idInteres { get; set; }

        [ForeignKey("Usuarias")]
        public int Fk_idUsuaria { get; set; }
    }
}
