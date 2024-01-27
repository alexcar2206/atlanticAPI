using FMJ_Backend.JSONS;

namespace FMJ_Backend.Modelos
{
    public class UsuariasDto
    {

        public int Id_usuaria { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Contra { get; set; }
        public int Edad { get; set; }
        public List<int>? interesesID { get; set; }
        public Ubicacion? Ubicacion { get; set; }
        public string? DNI { get; set; }
        public DateTime ValidezDNI { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int CodPostal { get; set; }
        public bool Socia { get; set; }
        public int? Fk_IdAsociacion { get; set; }
        public string? Bio { get; set; }
        public Referentas[]? Referentas { get; set; }
        public int Fk_idInsignia { get; set; }
        public int Fk_idRol { get; set; }
        public string? fotoPerfil { get; set; }
        public string? fotoPerfilContentType { get; set; }
    }
}