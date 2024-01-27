using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend.Controllers.DatosEndpoints.PUT
{
    public class datosUpdatePublicacion
    {
        public int id { get; set; }
        public FotosDto fotoNueva { get; set; }
        public FotosDto fotoMinNueva { get; set; }
        public PublicacionesDto publicacionDto { get; set; }
    }
}
