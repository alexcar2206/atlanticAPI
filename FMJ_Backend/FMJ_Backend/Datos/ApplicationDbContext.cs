using FMJ_Backend.Modelos;
using FMJ_Backend.Modelos.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FMJ_Backend.Datos
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }


        
        public DbSet<Roles> roles { get; set; }
        public DbSet<Fotos> fotos { get; set; }
        public DbSet<Intereses> intereses { get; set; }
        public DbSet<Asociaciones> asociaciones { get; set; }
        public DbSet<Insignias> insignias { get; set; }
        public DbSet<Usuarias> usuarias { get; set; }
        public DbSet<Publicaciones> publicaciones { get; set; } 
        public DbSet<InteresesUsuarias> interesesUsuarias { get; set; }
        public DbSet<Estados> estados { get; set; }
        public DbSet<Cargos> cargos { get; set; }
        public DbSet<Categorias> categorias { get; set; }
        public DbSet<CategoriaArchivo> categoriaArchivo { get; set; }
        public DbSet<ArchivoRepositorio> archivoRepositorio { get; set; }
        public DbSet<Repositorio> repositorio {  get; set; }
             

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<Roles>().HasData(
                new Roles()
                {
                    Id_rol = 1,
                    Nombre = "ADM",
                    Accesos = "",
                    Descripcion = ""
                },
                new Roles()
                {
                    Id_rol = 2,
                    Nombre = "JD",
                    Accesos = "",
                    Descripcion = ""
                },
                new Roles()
                {
                    Id_rol = 3,
                    Nombre = "JT",
                    Accesos = "",
                    Descripcion = ""
                },
                new Roles()
                {
                    Id_rol = 4,
                    Nombre = "SOC",
                    Accesos = "",
                    Descripcion = ""
                },
                new Roles()
                {
                    Id_rol = 5,
                    Nombre = "EXT",
                    Accesos = "",
                    Descripcion = ""
                }
                );


            modelBuilder.Entity<Fotos>().HasData(
                new Fotos()
                {
                    Id_Foto = 1,
                    Fk_idUsuaria = 1,
                    Foto = "",
                    Tipo = "PERFIL"

                },
                new Fotos()
                {
                    Id_Foto = 2,
                    Fk_idUsuaria = 1,
                    Foto = "",
                    Tipo = "DNI"
                },
           
                new Fotos()
                {
                    Id_Foto = 4,
                    Fk_idUsuaria = 1,
                    Foto = "",
                    Tipo = "PUBLICACION"
                },
                new Fotos()
                {
                    Id_Foto = 5,
                    Fk_idUsuaria = 1,
                    Foto = "",
                    Tipo = "PUBLICACION"
                },
                new Fotos()
                {
                    Id_Foto = 6,
                    Fk_idUsuaria = 2,
                    Foto ="",
                    Tipo = "PERFIL"

                },
                new Fotos()
                {
                    Id_Foto = 7,
                    Fk_idUsuaria = 2,
                    Foto ="",
                    Tipo = "DNI"
                },
 
                new Fotos()
                {
                    Id_Foto = 9,
                    Fk_idUsuaria = 2,
                    Foto = "",
                    Tipo = "PUBLICACION"
                },
                new Fotos()
                {
                    Id_Foto = 10,
                    Fk_idUsuaria = 2,
                    Foto = "",
                    Tipo = "PUBLICACION"
                }

                ) ;




            modelBuilder.Entity<Intereses>().HasData(
                new Intereses()
                {
                    Id_interes = 1,
                    Nombre = "Cine",
                    Descripcion = ""
                },
                new Intereses()
                {
                    Id_interes = 2,
                    Nombre = "Música",
                    Descripcion = ""
                },
                new Intereses()
                {
                    Id_interes = 3,
                    Nombre = "Juegos de mesa",
                    Descripcion = ""
                },
                new Intereses()
                {
                    Id_interes = 4,
                    Nombre = "Electrónica",
                    Descripcion = ""
                }
                );



            modelBuilder.Entity<Asociaciones>().HasData(
               new Asociaciones()
               {
                   Id_asociacion = 1,
                   Nombre = "Asociacion 1",
                   Descripcion = "",
                   Ubicacion = "",
                   Registration_date = DateTime.Now,
                   
                  
               },
               new Asociaciones()
               {
                   Id_asociacion = 2,
                   Nombre = "Asociacion 2",
                   Descripcion = "",
                   Ubicacion = "",
                   Registration_date = DateTime.Now,
               },
               new Asociaciones()
               {
                   Id_asociacion = 3,
                   Nombre = "Asociacion 3",
                   Descripcion = "",
                   Ubicacion = "",
                   Registration_date = DateTime.Now,
               }
               );



            modelBuilder.Entity<Insignias>().HasData(
                new Insignias()
                {
                    Id_insignia = 1,
                    Nombre = "Insignia 1",
                    Descripcion = "Insignia numero 1",
                    Icon = ""


                },
                new Insignias()
                {
                    Id_insignia = 2,
                    Nombre = "Insignia 2",
                    Descripcion = "Insignia numero 2",
                    Icon = ""
                },
                new Insignias()
                {
                    Id_insignia = 3,
                    Nombre = "Insignia 3",
                    Descripcion = "Insignia numero 3",
                    Icon = ""
                });


            modelBuilder.Entity<Usuarias>().HasData(
                new Usuarias()
                {
                    Id_usuaria = 1,
                    Nombre = "Sara",
                    Apellidos = "Martinez",
                    NickName = "Sara22",
                    Email = "sara22@gmail.com",
                    Contra = "1234",
                    TokenRecuperacionContrasena = "",
                    DNI = "5385752N",
                    ValidezDNI = new DateTime(2027,06,1),
                    FechaNacimiento = new DateTime(2001,06,22),
                    CodPostal = 36209,
                    Socia = true,
                    Bio = "descripcion",
                    Fk_IdAsociacion = 1,
                    Fk_idInsignia = 1,
                    Edad = 22,
                    Referentas = "",
                    Fk_idRol = 1,
                    Ubicacion = "",
                    Validado = true,
                    Validation_date = new DateTime(1111,1,1),
                    Validation_userid = 0,
                    Registration_date = DateTime.Now,
                    Modification_date = DateTime.Now,
                    Condiciones = false
                    
                },
                new Usuarias()
                {
                    Id_usuaria = 2,
                    Nombre = "Lucía",
                    Apellidos = "Fernandez",
                    NickName = "Lucia34",
                    Email = "Lucia34@gmail.com",
                    Contra = "1234",
                    TokenRecuperacionContrasena = "",
                    DNI = "5385752N",
                    ValidezDNI = new DateTime(2027, 06, 1),
                    FechaNacimiento = new DateTime(1987, 02, 17),
                    CodPostal = 36209,
                    Socia = true,
                    Bio = "descripcion",
                    Fk_IdAsociacion = 1,
                    Fk_idInsignia = 3,
                    Edad = 34,
                    Referentas = "",
                    Fk_idRol = 1,
                    Ubicacion = "",
                    Validado = false,             
                    Validation_date = new DateTime(1111, 1, 1),
                    Validation_userid = 0,
                    Registration_date = DateTime.Now,
                    Modification_date = DateTime.Now,
                    Condiciones = false

                }
                );


            modelBuilder.Entity<Publicaciones>().HasData(
                new Publicaciones()
                {
                    Id_publicacion = 1,
                    Cuerpo = "",
                    Comentarios = "",
                    RegistrationDate = DateTime.Now,
                    ModificacionDate = DateTime.Now,
                    fk_idUsuaria = 1,
                    fk_idFoto = 4,
                    Reacciones = ""

                },
                new Publicaciones()
                {
                    Id_publicacion = 2,
                    Cuerpo = "",
                    Comentarios = "",
                    RegistrationDate = DateTime.Now,
                    ModificacionDate = DateTime.Now,
                    fk_idUsuaria = 1,
                    fk_idFoto = 5,
                    Reacciones = ""
                },
                new Publicaciones()
                {
                    Id_publicacion = 3,
                    Cuerpo = "",
                    Comentarios = "",
                    RegistrationDate = DateTime.Now,
                    ModificacionDate = DateTime.Now,
                    fk_idUsuaria = 2,
                    fk_idFoto = 9,
                    Reacciones = ""
                },
                new Publicaciones()
                {
                    Id_publicacion = 4,
                    Cuerpo = "",
                    Comentarios = "",
                    RegistrationDate = DateTime.Now,
                    ModificacionDate = DateTime.Now,
                    fk_idUsuaria = 2,
                    fk_idFoto = 10,
                    Reacciones = ""
                }
                );


            modelBuilder.Entity<InteresesUsuarias>().HasData(
                new InteresesUsuarias()
                {
                    Id_InteresesUsuarias = 1,
                    Fk_idUsuaria = 1,
                    Fk_idInteres = 2
                },
                new InteresesUsuarias()
                 {
                    Id_InteresesUsuarias = 2,
                    Fk_idUsuaria = 1,
                    Fk_idInteres = 4
                },
                new InteresesUsuarias()
                {
                    Id_InteresesUsuarias = 3,
                    Fk_idUsuaria = 2,
                    Fk_idInteres = 1
                },
                new InteresesUsuarias()
                {
                    Id_InteresesUsuarias = 4,
                    Fk_idUsuaria = 2,
                    Fk_idInteres = 3
                   }
                );
           
           
           
        }

    }
}
