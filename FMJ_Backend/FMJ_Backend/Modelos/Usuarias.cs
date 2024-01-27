using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FMJ_Backend.Modelos
{
    public class Usuarias
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_usuaria { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NickName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Contra { get; set; }
        public string? TokenRecuperacionContrasena { get; set; }   
        public int Edad { get; set; }
        public string? Ubicacion { get; set; }
        public string? DNI { get; set; }
        public DateTime? ValidezDNI { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int CodPostal { get; set; }
        public bool Socia { get; set; }
        public string? Bio { get; set; }
        public DateTime Registration_date { get; set; }
        public DateTime Modification_date { get; set; }
        public DateTime? Validation_date { get; set; }
        public bool Condiciones { get; set; }
        public bool Validado { get; set; }
        public int? Validation_userid { get; set; }
        public string? Referentas { get; set; }
        public string? Observaciones {  get; set; }


        [ForeignKey("Roles")]
        public int? Fk_idRol { get; set; }

        [ForeignKey("Insignias")]
        public int? Fk_idInsignia { get; set; }

        [ForeignKey("Asociaciones")]
        public int? Fk_IdAsociacion { get; set; }

        [ForeignKey("Estados")]
        public int? Estado { get; set; }

        [ForeignKey("Cargos")]
        public int? Cargo { get; set; }



    }
}
