using FMJ_Backend.Modelos.Dto;

namespace FMJ_Backend
{
    public class datosGenerateJwtTokenNew
    {
        public int Id_usuaria { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Contra { get; set; }
        public string Observaciones { get; set; }
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
        public int? Fk_idRol { get; set; }
        public List<InteresesDto> intereses { get; set; }
        public int? Estado { get; set; }
        public int? Cargo { get; set; }
    }
}
