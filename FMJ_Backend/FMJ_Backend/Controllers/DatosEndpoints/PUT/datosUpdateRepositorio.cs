namespace FMJ_Backend.Controllers.DatosEndpoints.PUT
{
    public class datosUpdateRepositorio
    {
        public int IdRepositorio { get; set; }
        public string? TituloArchivo { get; set; }
        public int IdUsuaria { get; set; }
        public string? Descripcion { get; set; }
        public int IdArchivoRepositorio { get; set; }
        public int DirigidoA { get; set; }
        public int IdCategoriaArchivo { get; set; }
        public string? FileName { get; set; }
    }
}
