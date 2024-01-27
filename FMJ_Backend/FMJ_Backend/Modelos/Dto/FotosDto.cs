using System.ComponentModel.DataAnnotations.Schema;

namespace FMJ_Backend.Modelos.Dto
{
    public class FotosDto
    {
        public string Foto { get; set; }
        public string? contentType { get; set; }

    }
}
