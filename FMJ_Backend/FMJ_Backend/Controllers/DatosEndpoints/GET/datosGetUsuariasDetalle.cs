using FMJ_Backend.JSONS;

namespace FMJ_Backend.Controllers.DatosEndpoints.GET
{
    public class datosGetUsuariasDetalle
    {
        public int id { get; set; }
        public string name { get; set; }
        public string apellido { get; set; }
        public string insignia { get; set; }
        public string nickName { get; set; }
        public Ubicacion ubicacion { get; set; }
        public string asociacion { get; set; }
        public string bio { get; set; }
        public List<Referentas> referentas { get; set; }
        public List<string> intereses { get; set; }
    }
}
