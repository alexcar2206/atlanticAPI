using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos
{
    public class Publicaciones
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_publicacion { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? NickName { get; set; }
        public string? DirigidoA { get; set; }
        public string? Insignia { get; set; }
        public string? Descripcion {  get; set; }
        public string? Cuerpo { get; set; }
        public bool PermiteComentarios { get; set; }
        public bool Visible { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ModificacionDate { get; set; }
        public string? Comentarios { get; set; }
        public string? Reacciones { get; set; }
        public string? Titulo {  get; set; }
        public string? Ubicacion {  get; set; }

        
        [ForeignKey("Usuarias")]
        public int fk_idUsuaria { get; set; }

        [ForeignKey("Fotos")]
        public int? fk_idFoto { get; set; }

        [ForeignKey("Fotos")]
        public int? fk_idFotoMin { get; set; }

        [ForeignKey("Categorias")]
        public int? fk_idCategoria { get; set; }

    }
}
