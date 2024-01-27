using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend.Controllers.DatosEndpoints.POST
{
    public class datosNuevoComentario
    {
        public int idPublicacion { get; set; }
        public ComentarioDto comentario { get; set; }
    }
}
