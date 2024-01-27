using Microsoft.EntityFrameworkCore;

namespace FMJ_Backend.JSONS
{
    public class Comentario
    {
        public int idComenatario { get; set; }
        public int idUser { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string nickname { get; set; }
        public string insignia { get; set; }
        public string comentario { get; set; }
        public DateTime registrationDate { get; set; }
    }
}
