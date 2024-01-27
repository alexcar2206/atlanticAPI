using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend.Controllers.DatosEndpoints.PUT
{
    public class datosUpdateEstado
    {
        public int idEstado { get; set; }
        public EstadosDto estado { get; set; }
    }
}
