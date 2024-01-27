using FMJ_Backend.Modelos;

namespace FMJ_Backend.Datos
{
    public class UserStore
    {
        public List<UsuariasDto> usuarias { get; set; }


        public UserStore() {

            usuarias = new List<UsuariasDto>();

            usuarias.Add(
                new UsuariasDto()
                {
                    Id_usuaria = 1,
                    Nombre = "asda",
                    NickName = "alex22",
                    Contra = "1234",
                    Apellidos = "asdsa",
                    Edad = 22
                }              

            );

            usuarias.Add(
                new UsuariasDto()
                {
                    Id_usuaria = 2,
                    Nombre = "ghjhjg",
                    NickName = "juan32",
                    Contra =  "4321",
                    Apellidos = "jghkgh",
                    Edad = 23
                }
            );


        }  


    }
}
