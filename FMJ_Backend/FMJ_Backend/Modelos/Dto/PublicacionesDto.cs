namespace FMJ_Backend.Modelos.Dto
{
    public class PublicacionesDto
    {
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public string? Descripcion { get; set; }
        public string Ubicacion {  get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NickName { get; set; }
        public List<int> DirigidoA { get; set; }
        public string Insignia { get; set; }
        public bool PermiteComentarios { get; set; }
        public int? fk_idCategoria { get; set; }
        public bool Visible { get; set; }
    }
}
