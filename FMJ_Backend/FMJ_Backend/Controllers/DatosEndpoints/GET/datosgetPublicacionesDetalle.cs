namespace FMJ_Backend.Controllers.DatosEndpoints.GET
{
    public class datosgetPublicacionesDetalle
    {
        public int id { get; set; }
        public string fotoPublicacion { get; set; }
        public string contentTypePublicacion { get; set; }
        public string fotoPublicacionMin { get; set; }
        public string contentTypePublicacionMin { get; set; }
        public string fotoPerfil { get; set; }
        public string contentTypePerfil { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string nickName { get; set; }
        public List<int> dirigidoA { get; set; }
        public string insignia { get; set; }
        public bool permiteComentarios { get; set; }
        public bool visible { get; set; }
        public string body { get; set; }
        public string? descripcion { get; set; }
        public int numComent { get; set; }
        public DateTime registratDate { get; set; }
        public DateTime modificationDate { get; set; }
        public string titulo { get; set; }
        public string ubicacion { get; set; }
        public int? idCategoria { get; set; }
    }
}
