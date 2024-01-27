using FMJ_Backend.JSONS;

namespace FMJ_Backend.Controllers.DatosEndpoints.POST
{
    public class datosAltaUsuaria
    {
        public int id { get; set; }
        public string dni { get; set; }
        public string? fotoDNI { get; set; }
        public DateTime validez { get; set; }
        public DateTime fechaNac { get; set; }
        public int codpostal { get; set; }
        public bool socia { get; set; }
        public int? asociacion { get; set; }
        public string bio { get; set; }
        public List<Referentas>? referencias { get; set; }
        public List<int>? intereses { get; set; }
        public string? fotoPerfil { get; set; }
        public string fotoPerfilContentType { get; set; }
        public string fotoDNIContentType { get; set; }
    }
}
