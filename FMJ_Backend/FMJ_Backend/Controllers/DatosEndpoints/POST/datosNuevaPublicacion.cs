using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend.Controllers.DatosEndpoints.POST
{
    public class datosNuevaPublicacion
    {
        public FotosDto foto { get; set; }
        public FotosDto fotoMin { get; set; }
        public int idUsuaria { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nickName { get; set; }
        public List<int> dirigidoA { get; set; }
        public string insignia { get; set; }
        public bool permiteComentarios { get; set; }
        public bool visible { get; set; }
        public string cuerpo { get; set; }
        public string descripcion { get; set; }
        public string titulo { get; set; }
        public string ubicacion { get; set; }

        public int idCategoria { get; set; }
    }
}
