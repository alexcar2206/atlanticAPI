using FMJ_Backend.Modelos.Dto;
using System.ComponentModel.DataAnnotations;

namespace FMJ_Backend.Controllers.DatosEndpoints.GET
{
    public class datosGetUsuariaById
    {
        public int Id_usuaria { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NickName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Contra { get; set; }
        public string? TokenRecuperacionContrasena { get; set; }
        public int Edad { get; set; }
        public string? Ubicacion { get; set; }
        public string? DNI { get; set; }
        public DateTime? ValidezDNI { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int CodPostal { get; set; }
        public bool Socia { get; set; }
        public int? Fk_IdAsociacion { get; set; }
        public string? Bio { get; set; }
        public DateTime Registration_date { get; set; }
        public DateTime Modification_date { get; set; }
        public DateTime? Validation_date { get; set; }
        public bool Condiciones { get; set; }
        public bool Validado { get; set; }
        public int? Validation_userid { get; set; }
        public string? Referentas { get; set; }
        public int? Fk_idInsignia { get; set; }
        public string insigniaIcon { get; set; }
        public int? Fk_idRol { get; set; }
        public string fotoPerfil { get; set; }
        public string fotoPerfilContentType { get; set; }
        public string fotoDNI { get; set; }
        public string fotoDNIContentType { get; set; }
        public List<InteresesDto> intereses { get; set; }
        public int? estado { get; set; }
    }
}
