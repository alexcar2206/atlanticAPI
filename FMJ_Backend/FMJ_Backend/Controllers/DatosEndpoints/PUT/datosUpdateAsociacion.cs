using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend.Controllers.DatosEndpoints.PUT
{
    public class datosUpdateAsociacion
    {
        public int id { get; set; }
        public AsociacionesDto asociacionDto { get; set; }
    }
}
