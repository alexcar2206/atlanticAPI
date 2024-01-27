using Castle.Core.Smtp;
using FMJ_Backend.Controllers.DatosEndpoints.DELETE;
using FMJ_Backend.Controllers.DatosEndpoints.POST;
using FMJ_Backend.Controllers.DatosEndpoints.PUT;
using FMJ_Backend.Controllers.DatosEndpoints.GET;
using FMJ_Backend.Datos;
using FMJ_Backend.Modelos;
using FMJ_Backend.Modelos.Dto;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using static System.Net.Mime.MediaTypeNames;
using FMJ_Backend.JSONS;
using FMJ_Backend.Constantes;

namespace FMJ_Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllOrigins")]


    public class FMJController : Controller
    {
        private readonly ILogger<FMJController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly ServicioCorreoConfig _configuracionCorreo;
        public Metodos metodos;


        public FMJController(ILogger<FMJController> logger, ApplicationDbContext db, IOptions<ServicioCorreoConfig> config)
        {
            _logger = logger;
            _db = db;
            _configuracionCorreo = config.Value;
            metodos = new Metodos(_configuracionCorreo);
        }




        /*
         * SERVICIO PARA OBTENER TODAS LAS CATEGORIAS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getCategorias()
        {
           
            List<Categorias> categorias = _db.categorias.ToList();

            if (categorias == null || categorias.Count == 0)
            {
                return NotFound("No hay categorías");
            }       

            return Ok(categorias);

        }


        /*
         * SERVICIO PARA OBTENER UNA CATEGORIA POR SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getCategoriaById(int id)
        {

            Categorias categoria = _db.categorias.FirstOrDefault(c => c.Id_Categoria == id);

            if (categoria == null)
            {
                return NotFound("No existe categoria con ese id");
            }

            return Ok(categoria);

        }




        /*
         * SERVICIO PARA OBTENER TODOS LOS COMENTARIOS PASASANDO ID DE PUBLICACION
         */
        [Authorize]
        [HttpGet("{idPubli:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getComentarios(int idPubli)
        {
            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == idPubli);

            if (publicacion == null)
            {
                return NotFound("No existe");
            }

            List<Comentario> comentarios = JsonConvert.DeserializeObject<List<Comentario>>(publicacion.Comentarios);
            List<datosgetComentarios> comentariosInfo = new List<datosgetComentarios>();


            if (comentarios == null)
            {
                return NotFound("Publicacion sin comentarios");
            }


            foreach (var comentario in comentarios)
            {
                Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == comentario.idUser);

                string nickName = "DELETE_USER";

                if (usuaria != null)
                {
                    nickName = usuaria.NickName;
                }


                datosgetComentarios comentarioInfo = new datosgetComentarios()
                {
                    id = comentario.idComenatario,
                    comentario = comentario.comentario,
                    usuario = nickName,
                    registrationDate = comentario.registrationDate
                };

                comentariosInfo.Add(comentarioInfo);
            }

            return Ok(comentariosInfo);
        }

        /*
         * SERVICIO PARA OBTENER TODAS LAS PUBLICACIONES
         */
        [Authorize]
        [HttpGet]
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> getPublicaciones()
        {
            List<Publicaciones> publicaciones = _db.publicaciones.ToList();
            List<datosgetPublicaciones> publicacionesInfo = new List<datosgetPublicaciones>();

            foreach (var publicacion in publicaciones)
            {

                string fotoPerf = "";
                string contentTypeFotoPefil = "";
                //string fotoPubli = "";
                //string contentTypeFotoPubli = "";
                string fotoPubliMin = "";
                string contentTypeFotoPubliMin = "";




                Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == publicacion.fk_idUsuaria);

                if (usuaria != null)
                {

                    Fotos fotoUser = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == usuaria.Id_usuaria && f.Tipo == tipoFoto.PERFIL);

                    if (fotoUser != null)
                    {
                        fotoPerf = fotoUser.Foto;
                        contentTypeFotoPefil = fotoUser.contentType;
                    }

                }

                /*
                Fotos foto = _db.fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);


                if (foto != null)
                {
                    fotoPubli = foto.Foto;
                    contentTypeFotoPubli = foto.contentType;
                }
                */

                Fotos fotoMin = _db.fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFotoMin);


                if (fotoMin != null)
                {
                    fotoPubliMin = fotoMin.Foto;
                    contentTypeFotoPubliMin = fotoMin.contentType;
                }




                datosgetPublicaciones publicacionInfo = new datosgetPublicaciones()
                {
                    id = publicacion.Id_publicacion,
                    body = publicacion.Cuerpo,
                    descripcion = publicacion.Descripcion,
                    nickName = publicacion.NickName,
                    nombre = publicacion.Nombre,
                    apellidos = publicacion.Apellidos,
                    dirigidoA = JsonConvert.DeserializeObject<List<int>>(publicacion.DirigidoA),
                    insignia = publicacion.Insignia,
                    permiteComentarios = publicacion.PermiteComentarios,
                    visible = publicacion.Visible,
                    fotoPerfil = fotoPerf,
                    contentTypePerfil = contentTypeFotoPefil,
                    //fotoPublicacion = fotoPubli,
                    //contentTypePublicacion = contentTypeFotoPubli,
                    fotoPublicacionMin = fotoPubliMin,
                    contentTypePublicacionMin = contentTypeFotoPubliMin,
                    ubicacion = publicacion.Ubicacion,
                    titulo = publicacion.Titulo,
                    idCategoria = publicacion.fk_idCategoria

                };

                publicacionesInfo.Add(publicacionInfo);

            }
            return Ok(publicacionesInfo);
        }





        /*
        * SERVICIO PARA OBTENER TODAS LAS PUBLICACIONES
        */
        
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult> getPublicacionesNew()
        {
            List<Publicaciones> publicaciones = _db.publicaciones.ToList();

            //Lista de ids de usuarios que tienen publicaciones
            List<int> idsUser = new List<int>();


            //Relleno de la lista idsUser
            foreach (var publicacion in publicaciones)
            {
                int idUser = publicacion.fk_idUsuaria;

                if (!idsUser.Contains(idUser))
                {
                    idsUser.Add(idUser);
                }
            }


            //Fotos de perfil de usuarios que tienen publiaciones
            List<Fotos> fotosPerfil = _db.fotos.Where(f => f.Tipo == tipoFoto.PERFIL && idsUser.Contains(f.Fk_idUsuaria)).ToList();

            //Fotos de las publicaciones
            //List<Fotos> fotosPublicaciones = _db.fotos.Where(f => f.Tipo == tipoFoto.PUBLICACION).ToList();

            //Fotos de las publicaciones min
            List<Fotos> fotosPublicacionesMin = _db.fotos.Where(f => f.Tipo == tipoFoto.PUBLICACION_MIN).ToList();


            Fotos fotoUsuaria = new Fotos();
            //Fotos fotoPublicacion = new Fotos();
            Fotos fotoPublicacionMin = new Fotos();

            List<datosgetPublicaciones> publicacionesInfo = new List<datosgetPublicaciones>();


            foreach (var publicacion in publicaciones)
            {

                string fotoPerf = "";
                string contentTypeFotoPefil = "";
                string fotoPubli = "";
                string contentTypeFotoPubli = "";
                string fotoPubliMin = "";
                string contentTypeFotoPubliMin = "";


                //Ahora para obtener foto de perfil se obtiene de la lista fotosPerfil en vez de hacer consulta a BBDD
                fotoUsuaria = fotosPerfil.FirstOrDefault(f => f.Fk_idUsuaria == publicacion.fk_idUsuaria);

                if (fotoUsuaria != null)
                {
                    fotoPerf = fotoUsuaria.Foto;
                    contentTypeFotoPefil = fotoUsuaria.contentType;
                }


                //Ahora para obtener foto de publicacion se obtiene de la lista fotosPublicaciones en vez de hacer consulta a BBDD
                /*fotoPublicacion = fotosPublicaciones.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);

                if (fotoPublicacion != null)
                {
                    fotoPubli = fotoPublicacion.Foto;
                    contentTypeFotoPubli = fotoPublicacion.contentType;
                }*/


                //Ahora para obtener foto de publicacionMin se obtiene de la lista fotosPublicacionesMin en vez de hacer consulta a BBDD
                fotoPublicacionMin = fotosPublicacionesMin.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFotoMin);

                if (fotoPublicacionMin != null)
                {
                    fotoPubliMin = fotoPublicacionMin.Foto;
                    contentTypeFotoPubliMin = fotoPublicacionMin.contentType;
                }




                datosgetPublicaciones publicacionInfo = new datosgetPublicaciones()
                {
                    id = publicacion.Id_publicacion,
                    body = publicacion.Cuerpo,
                    descripcion = publicacion.Descripcion,
                    nickName = publicacion.NickName,
                    nombre = publicacion.Nombre,
                    apellidos = publicacion.Apellidos,
                    dirigidoA = JsonConvert.DeserializeObject<List<int>>(publicacion.DirigidoA),
                    insignia = publicacion.Insignia,
                    permiteComentarios = publicacion.PermiteComentarios,
                    visible = publicacion.Visible,
                    fotoPerfil = fotoPerf,
                    contentTypePerfil = contentTypeFotoPefil,
                    //fotoPublicacion = fotoPubli,
                    //contentTypePublicacion = contentTypeFotoPubli,
                    fotoPublicacionMin = fotoPubliMin,
                    contentTypePublicacionMin = contentTypeFotoPubliMin,
                    ubicacion = publicacion.Ubicacion,
                    titulo = publicacion.Titulo,
                    idCategoria = publicacion.fk_idCategoria

                };

                publicacionesInfo.Add(publicacionInfo);

            }
            return Ok(publicacionesInfo);
        }

        /*
         * SERVICIO PARA OBTENER PUBLICACIONES DIRIGIDAS A LOS NÚMEROS QUE SE LE PASE COMO ARRAY
         */
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult> getPublicacionesDirigidasA(List<int> users)
        {
            List<Publicaciones> publicaciones = _db.publicaciones.AsEnumerable().Where(p => JsonConvert.DeserializeObject<List<int>>(p.DirigidoA).Intersect(users).Any()).ToList();

            //Lista de ids de usuarios que tienen publicaciones
            List<int> idsUser = new List<int>();
            List<int?> idsFotoMin = new List<int?>();


            //Relleno de la lista idsUser
            foreach (var publicacion in publicaciones)
            {

                int idUser = publicacion.fk_idUsuaria;

                if (!idsUser.Contains(idUser))
                {
                    idsUser.Add(idUser);
                }

                if (publicacion.fk_idFotoMin!=null)
                {
                    idsFotoMin.Add(publicacion.fk_idFotoMin);
                }

                
                
            }


            //Fotos de perfil de usuarios que tienen publiaciones
            List<Fotos> fotosPerfil = _db.fotos.Where(f => f.Tipo == tipoFoto.PERFIL && idsUser.Contains(f.Fk_idUsuaria)).ToList();

            //Fotos de las publicaciones
           // List<Fotos> fotosPublicaciones = _db.fotos.Where(f => idsFoto.Contains(f.Id_Foto)).ToList();

            //Fotos de las publicaciones min
            //List<Fotos> fotosPublicacionesMin = _db.fotos.Where(f => f.Tipo == tipoFoto.PUBLICACION_MIN).ToList();
            List<Fotos> fotosPublicacionesMin = _db.fotos.Where(f => idsFotoMin.Contains(f.Id_Foto)).ToList();


            Fotos fotoUsuaria = new Fotos();
            //Fotos fotoPublicacion = new Fotos();
            Fotos fotoPublicacionMin = new Fotos();

            List<datosgetPublicaciones> publicacionesInfo = new List<datosgetPublicaciones>();


            foreach (var publicacion in publicaciones)
            {

                string fotoPerf = "";
                string contentTypeFotoPefil = "";
                //string fotoPubli = "";
                //string contentTypeFotoPubli = "";
                string fotoPubliMin = "";
                string contentTypeFotoPubliMin = "";


                //Ahora para obtener foto de perfil se obtiene de la lista fotosPerfil en vez de hacer consulta a BBDD
                fotoUsuaria = fotosPerfil.FirstOrDefault(f => f.Fk_idUsuaria == publicacion.fk_idUsuaria);

                if (fotoUsuaria != null)
                {
                    fotoPerf = fotoUsuaria.Foto;
                    contentTypeFotoPefil = fotoUsuaria.contentType;
                }


                //Ahora para obtener foto de publicacion se obtiene de la lista fotosPublicaciones en vez de hacer consulta a BBDD
                /*fotoPublicacion = fotosPublicaciones.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);

                if (fotoPublicacion != null)
                {
                    fotoPubli = fotoPublicacion.Foto;
                    contentTypeFotoPubli = fotoPublicacion.contentType;
                }
                */
                //Ahora para obtener foto de publicacion se obtiene de la lista fotosPublicaciones en vez de hacer consulta a BBDD

                fotoPublicacionMin = fotosPublicacionesMin.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFotoMin);

                if (fotoPublicacionMin != null)
                {
                    fotoPubliMin = fotoPublicacionMin.Foto;
                    contentTypeFotoPubliMin = fotoPublicacionMin.contentType;
                }


                datosgetPublicaciones publicacionInfo = new datosgetPublicaciones()
                {
                    id = publicacion.Id_publicacion,
                    body = publicacion.Cuerpo,
                    nickName = publicacion.NickName,
                    nombre = publicacion.Nombre,
                    apellidos = publicacion.Apellidos,
                    dirigidoA = JsonConvert.DeserializeObject<List<int>>(publicacion.DirigidoA),
                    insignia = publicacion.Insignia,
                    permiteComentarios = publicacion.PermiteComentarios,
                    visible = publicacion.Visible,
                    fotoPerfil = fotoPerf,
                    contentTypePerfil = contentTypeFotoPefil,
                    //fotoPublicacion = fotoPubli,
                    //contentTypePublicacion = contentTypeFotoPubli,
                    fotoPublicacionMin = fotoPubliMin,
                    contentTypePublicacionMin = contentTypeFotoPubliMin,
                    ubicacion = publicacion.Ubicacion,
                    titulo = publicacion.Titulo

                };

                publicacionesInfo.Add(publicacionInfo);

            }
            return Ok(publicacionesInfo);
        }


        /*
         * SERVICIO PARA OBTENER LAS PUBLICACIONES CON MÁS DETALLES
         */
        [Authorize]
        [HttpGet]
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Publicaciones>> getPublicacionesDetalle()
        {

            List<int> idsUser = new List<int>();
            List<int?> idsFotos = new List<int?>();
            List<int?> idsFotosMin = new List<int?>();

            List<Publicaciones> publicaciones = _db.publicaciones.ToList();
           
            foreach (var publicacion in publicaciones)
            {
                int idUser = publicacion.fk_idUsuaria;
                int? idFoto = publicacion.fk_idFoto;
                int? idFotoMin = publicacion.fk_idFotoMin;

                if (!idsUser.Contains(idUser))
                {
                    idsUser.Add(idUser);
                }

                if (idFoto != null)
                {
                    idsFotos.Add(idFoto);
                }

                if (idFotoMin != null)
                {
                    idsFotosMin.Add(idFotoMin);
                }
            }

            List<Usuarias> usuarias = _db.usuarias.Where(u => idsUser.Contains(u.Id_usuaria)).ToList();
            List<Fotos> fotosUser = _db.fotos.Where(f => idsUser.Contains(f.Fk_idUsuaria) && f.Tipo == tipoFoto.PERFIL).ToList();
            List<Fotos> fotos = _db.fotos.Where(f => idsFotos.Contains(f.Id_Foto)).ToList();
            List<Fotos> fotosMin = _db.fotos.Where(f => idsFotosMin.Contains(f.Id_Foto)).ToList();


            List<datosgetPublicacionesDetalle> publicacionesInfo = new List<datosgetPublicacionesDetalle>();

            foreach (var publicacion in publicaciones)
            {

                string fotoPerf = "";
                string contenTypeFotoPerf = "";
                string fotoPubli = "";
                string contentTypePubli = "";
                string fotoPubliMin = "";
                string contentTypePubliMin = "";


                Usuarias usuaria = usuarias.FirstOrDefault(u => u.Id_usuaria == publicacion.fk_idUsuaria);

                if (usuaria != null)
                {
                    Fotos fotoUser = fotosUser.FirstOrDefault(f => f.Fk_idUsuaria == usuaria.Id_usuaria);

                    if (fotoUser != null)
                    {
                        fotoPerf = fotoUser.Foto;
                        contenTypeFotoPerf = fotoUser.contentType;
                    }
                    
                }

                Fotos foto = fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);


                if (foto != null)
                {
                    fotoPubli = foto.Foto;
                    contentTypePubli = foto.contentType;
                }

                Fotos fotoMin = fotosMin.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFotoMin);


                if (fotoMin != null)
                {
                    fotoPubliMin = fotoMin.Foto;
                    contentTypePubliMin = fotoMin.contentType;
                }

                List<Comentario> comentarios = JsonConvert.DeserializeObject<List<Comentario>>(publicacion.Comentarios);
                int numComentarios = 0;

                if (comentarios != null)
                {
                    numComentarios = comentarios.Count();
                }



                datosgetPublicacionesDetalle publicacionInfo = new datosgetPublicacionesDetalle()
                {
                    id = publicacion.Id_publicacion,
                    body = publicacion.Cuerpo,
                    nickName = publicacion.NickName,
                    nombre = publicacion.Nombre,
                    apellidos = publicacion.Apellidos,
                    dirigidoA = JsonConvert.DeserializeObject<List<int>>(publicacion.DirigidoA),
                    insignia = publicacion.Insignia,
                    permiteComentarios = publicacion.PermiteComentarios,
                    visible = publicacion.Visible,
                    fotoPerfil = fotoPerf,
                    contentTypePerfil = contenTypeFotoPerf,
                    fotoPublicacion = fotoPubli,
                    contentTypePublicacion = contentTypePubli,
                    numComent = numComentarios,
                    registratDate = publicacion.RegistrationDate,
                    modificationDate = publicacion.ModificacionDate,
                    titulo = publicacion.Titulo,
                    ubicacion = publicacion.Ubicacion

                };

                publicacionesInfo.Add(publicacionInfo);

            }

            return Ok(publicacionesInfo);
        }


        /*
        * SERVICIO PARA OBTENER PUBLICACION SEGUN SU ID
        */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getPublicacion(int id)
        {
            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == id);

            if (publicacion == null)
            {
                return NotFound();
            }

            return Ok(publicacion);
        }


        /*
         * SERVICIO PARA OBTENER TODAS LAS ASOCIACIONES
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getAsociaciones()
        {

            List<Asociaciones> asociaciones = _db.asociaciones.ToList();

            if (asociaciones == null)
            {
                return NotFound();
            }

            return Ok(asociaciones);
        }

        /*
         * SERVICIO PARA OBTENER ASOCIACION SEGUN SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getAsociacion(int id)
        {

            Asociaciones asociacion = _db.asociaciones.FirstOrDefault(a => a.Id_asociacion == id);

            if (asociacion == null)
            {
                return NotFound();
            }

            return Ok(asociacion);
        }


        /*
         * SERVICIO PARA OBTRENER TODOS LOS INTERESES
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getIntereses()
        {
            List<Intereses> intereses = _db.intereses.ToList();

            if (intereses == null)
            {
                return NotFound();
            }

            return Ok(intereses);
        }

        /*
         * SERVICIO PARA OBTENER INTERES SEGUN SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getInteres(int id)
        {
            Intereses interes = _db.intereses.FirstOrDefault(i => i.Id_interes == id);

            if (interes == null)
            {
                return NotFound();
            }

            return Ok(interes);
        }



        /*
         * SERVICIO PARA OBTRENER TODAS LAS INSIGNIAS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getInsignias()
        {
            List<Insignias> insignias = _db.insignias.ToList();

            if (insignias == null)
            {
                return NotFound();
            }

            return Ok(insignias);
        }

        /*
         * SERVICIO PARA OBTENER INSIGNIA SEGUN SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getInsignia(int id)
        {
            Insignias insignia = _db.insignias.FirstOrDefault(i => i.Id_insignia == id);

            if (insignia == null)
            {
                return NotFound();
            }

            return Ok(insignia);
        }


        /*
         * SERVICIO PARA OBTERNER TODOS LOS REPOSITORIOS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getRepositorios()
        {
            List<Repositorio> repositorios = _db.repositorio.ToList();

            if (repositorios == null || repositorios.Count == 0)
            {
                return NotFound("No existen repositorios");
            }

            return Ok(repositorios);    
        }


        /*
         * SERVICIO PARA OBTERNER UN REPOSITORIO CON ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getRepositorioById(int id)
        {
            Repositorio repositorio = _db.repositorio.FirstOrDefault(r => r.IdArchivoRepositorio == id);

            if (repositorio == null)
            {
                return NotFound("No existen repositorios con ese id");
            }

            return Ok(repositorio);
        }


        /*
         * SERVICIO PARA OBTENER TODOS LOS ARCHIVOS REPOSITORIO
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getArchivosRepositorio()
        {
            List<ArchivoRepositorio> archivos = _db.archivoRepositorio.ToList();

            if (archivos == null || archivos.Count == 0)
            {
                return NotFound("No hay archivos");
            }

            return Ok(archivos);    

        }



        /*
        * SERVICIO PARA OBTENER UN ARCHIVO REPOSITORIO SEGUN SU ID
        */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getArchivoRepositorioByiD(int id)
        {
           ArchivoRepositorio archivo = _db.archivoRepositorio.FirstOrDefault(a => a.IdArchivoRepositorio == id);

            if (archivo == null)
            {
                return NotFound("No hay archivo con ese id");
            }

            return Ok(archivo);

        }


        /*
         * SERVICIO PARA OBTENRER TODAS LAS CATEGORIAS ARCHIVO
         */
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getCategoriasArchivo()
        {
            List<CategoriaArchivo> categoriasArchivo = _db.categoriaArchivo.ToList();

            if (categoriasArchivo == null || categoriasArchivo.Count == 0)
            {
                return NotFound("No hay categorias");
            }

            return Ok(categoriasArchivo);
        }


        /*
         * SERVICIO PARA OBTERNER UNA CATEGORIA ARCHIVO SEGUN SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> getCategoriaArchivoById(int id)
        {
            CategoriaArchivo categoria = _db.categoriaArchivo.FirstOrDefault(c => c.IdCategoriaArchivo == id);

            if (categoria == null)
            {
                return NotFound("No hay categoria con ese id");
            }


            return Ok(categoria);
        }


        /*
         * SERVICIO PARA OBTENER TODOS LAS USUARIAS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<UsuariasDto>> getUsuarias()
        {

            List<Usuarias> usuarias = _db.usuarias.ToList();
            List<datosGetUsuarias> usuariasInfo = new List<datosGetUsuarias>();

            foreach (var usuaria in usuarias)
            {
                Asociaciones asociacion = _db.asociaciones.FirstOrDefault(a => a.Id_asociacion == usuaria.Fk_IdAsociacion);
                string nombreAsociacion = "";
                if (asociacion != null)
                {
                    nombreAsociacion = asociacion.Nombre;
                }

                datosGetUsuarias usuariainfo = new datosGetUsuarias()
                {
                    id = usuaria.Id_usuaria,
                    nickName = usuaria.NickName,
                    ubicacion = usuaria.Ubicacion,


                    asociacion = nombreAsociacion
                };

                usuariasInfo.Add(usuariainfo);
            }

            return Ok(usuariasInfo);
        }



        /*
         * SERVICIO PARA OBTENER USUARIAS CON MÁS DETALLES
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult> getUsuariasDetalle()
        {
            List<Usuarias> usuarias = _db.usuarias.ToList();

            List<datosGetUsuariasDetalle> usuariasDetalle = new List<datosGetUsuariasDetalle>();

            foreach (var usuaria in usuarias)
            {

                Asociaciones asociacion = _db.asociaciones.FirstOrDefault(a => a.Id_asociacion == usuaria.Fk_IdAsociacion);

                string nombreAsociacion = "";

                if (asociacion != null)
                {
                    nombreAsociacion = asociacion.Nombre;
                }

                List<InteresesUsuarias> intereses = _db.interesesUsuarias.Where(i => i.Fk_idUsuaria == usuaria.Id_usuaria).ToList();

                List<string> interesesInfo = new List<string>();

                foreach (var interesUsuaria in intereses)
                {
                    Intereses interes = _db.intereses.FirstOrDefault(i => i.Id_interes == interesUsuaria.Fk_idInteres);
                    string interesinfo = interes.Nombre;
                    interesesInfo.Add(interesinfo);
                }

                List<Referentas> referentasList = new List<Referentas>();

                if (usuaria.Referentas != null)
                {
                    referentasList = JsonConvert.DeserializeObject<List<Referentas>>(usuaria.Referentas);
                }

                Ubicacion ubicacion = new Ubicacion();

                if (usuaria.Ubicacion != null)
                {
                    ubicacion = JsonConvert.DeserializeObject<Ubicacion>(usuaria.Ubicacion);
                }

                string insigniaIcon = "";

                Insignias insignia = _db.insignias.FirstOrDefault(i => i.Id_insignia == usuaria.Fk_idInsignia);

                if (insignia != null)
                {
                    insigniaIcon = insignia.Icon;
                }



                datosGetUsuariasDetalle usuariaDetalle = new datosGetUsuariasDetalle()
                {
                    id = usuaria.Id_usuaria,
                    name = usuaria.Nombre,
                    apellido = usuaria.Apellidos,
                    nickName = usuaria.NickName,
                    ubicacion = ubicacion,
                    asociacion = nombreAsociacion,
                    referentas = referentasList,
                    intereses = interesesInfo,
                    insignia = insigniaIcon

                };

                usuariasDetalle.Add(usuariaDetalle);
            }

            return Ok(usuariasDetalle);
        }






        /*
         * SERVICIO PARA OBTENER UNA USUARIA EN CONCRETO PASANDOLE EL ID
         */
        //[Authorize]
        [HttpGet("{id:int}", Name = "GetUsuariaById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuariasDto>> getUsuariaById(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Error al traer usuaria con el id " + id);
                return BadRequest();
            }

            var usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == id);

            if (usuaria == null)
            {
                return NotFound();
            }

            Fotos fotoPerfil = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == id && f.Tipo == tipoFoto.PERFIL);

            string foto = "";
            string contentType = "";

            if (fotoPerfil != null)
            {
                foto = fotoPerfil.Foto;
                contentType = fotoPerfil.contentType;
            }


            Fotos fotoDNI = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == id && f.Tipo == tipoFoto.DNI);

            string fotoDNIbyte = "";
            string contentTypeDNI = "";

            if (fotoDNI != null)
            {
                fotoDNIbyte = fotoDNI.Foto;
                contentTypeDNI = fotoDNI.contentType;
            }


            List<InteresesUsuarias> interesesUsuarias = _db.interesesUsuarias.Where(i => i.Fk_idUsuaria == id).ToList();
            List<InteresesDto> interesesDto = new List<InteresesDto>();

            if (interesesUsuarias != null)
            {
                foreach (var interes in interesesUsuarias)
                {
                    Intereses interesdb = _db.intereses.FirstOrDefault(i => i.Id_interes == interes.Fk_idInteres);

                    InteresesDto intereses = new InteresesDto
                    {
                        Nombre = interesdb.Nombre,
                        Descripcion = interesdb.Descripcion
                    };

                    interesesDto.Add(intereses);
                }
            }



            datosGetUsuariaById datos = new datosGetUsuariaById()
            {
                Id_usuaria = id,
                Nombre = usuaria.Nombre,
                Apellidos = usuaria.Apellidos,
                NickName = usuaria.NickName,
                Email = usuaria.Email,
                Contra = usuaria.Contra,
                TokenRecuperacionContrasena = usuaria.TokenRecuperacionContrasena,
                Edad = usuaria.Edad,
                Ubicacion = usuaria.Ubicacion,
                DNI = usuaria.DNI,
                ValidezDNI = usuaria.ValidezDNI,
                FechaNacimiento = usuaria.FechaNacimiento,
                CodPostal = usuaria.CodPostal,
                Socia = usuaria.Socia,
                Fk_IdAsociacion = usuaria.Fk_IdAsociacion,
                Bio = usuaria.Bio,
                Registration_date = usuaria.Registration_date,
                Modification_date = usuaria.Modification_date,
                Validation_date = usuaria.Validation_date,
                Condiciones = usuaria.Condiciones,
                Validado = usuaria.Validado,
                Validation_userid = usuaria.Validation_userid,
                Referentas = usuaria.Referentas,
                Fk_idInsignia = usuaria.Fk_idInsignia,
                Fk_idRol = usuaria.Fk_idRol,
                fotoPerfil = foto,
                fotoPerfilContentType = contentType,
                fotoDNI = fotoDNIbyte,
                fotoDNIContentType = contentTypeDNI,
                intereses = interesesDto,
                estado = usuaria.Estado

            };


            return Ok(datos);
        }



        /*
         * SERVICIO PARA BUSCAR UNA USUARIA POR NOMBRE/NICKNAME       
         */
        [Authorize]
        [HttpGet("{nombre}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Usuarias>> getUsuariaByName(string nombre)
        {
            var usuaria = _db.usuarias.FirstOrDefault(u => u.Nombre.ToLower() == nombre.ToLower() || u.NickName.ToLower() == nombre.ToLower());

            if (usuaria == null)
            {
                return NotFound(nombre);
            }

            return Ok(usuaria.Nombre);
        }


        /*
         * SERVICIO PARA OBTENER TODAS LAS USUARIAS CON UN ROL DIFERENTE A "SOCIA""
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Usuarias>> getUsuariasNotNormal()
        {
            List<Usuarias> usuarias = _db.usuarias.Where(u => u.Fk_idRol != (int)tipoEstado.VALIDADA).ToList();

            if (usuarias == null)
            {
                return NotFound();
            }

            return Ok(usuarias);
        }


        /*
        * SERVICIO PARA OBTENER TODAS LAS USUARIAS CON UN ROL DE "NORMAL"
        */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Usuarias>> getUsuariasNormal()
        {
            List<Usuarias> usuarias = _db.usuarias.Where(u => u.Fk_idRol == (int)tipoEstado.VALIDADA).ToList();

            if (usuarias == null)
            {
                return NotFound();
            }

            return Ok(usuarias);
        }


        /*
         * SERVICIO PARA OBTENER USUARIAS SEGÚN SU ESTADO
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Usuarias>> getUsuariasByEstado(int idEstado)
        {
            List<Usuarias> usuarias = _db.usuarias.Where(u => u.Estado == idEstado).ToList();

            if (usuarias == null)
            {
                return NotFound();
            }

            return Ok(usuarias);
        }

        /*
         * SERVICIO PARA OBTENER TODAS LAS USUARIAS CON CAMPO VALIDADO A FALSE
         */
        //[Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Usuarias>>> getUsuariasInvalidadas()
        {
            List<Usuarias> usuarias = _db.usuarias.Where(u => u.Validado == false && u.Estado == (int)tipoEstado.PENDIENTE).ToList();

            if (usuarias == null)
            {
                return NotFound();
            }

            return Ok(usuarias);
        }

        /*
         * SERVICIO PARA OBTENER TODOS LOS ROLES
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Roles>> getRoles()
        {
            List<Roles> roles = _db.roles.ToList();
            if (roles == null)
            {
                return NotFound();
            }

            return Ok(roles);
        }



        /*
         * SERVICIO PARA OBTENER LOS ESTADOS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Estados>> getEstados()
        {
            List<Estados> estados = _db.estados.ToList();

            if (estados == null)
            {
                return NotFound();
            }

            return Ok(estados);
        }


        /*
        * SERVICIO PARA OBTENER UN ESTADO SEGÚN SU ID
        */
        [HttpGet]
        [Authorize]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Estados>> getEstadoById(int id)
        {
            Estados estado = _db.estados.FirstOrDefault(e => e.id_Estado == id);

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }



        /*
         * SERVICIO PARA OBTENER TODOS LOS CARGOS
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Cargos>>> getCargos()
        {
            List<Cargos> cargos = _db.cargos.ToList();

            if (cargos == null)
            {
                return NotFound();
            }

            return Ok(cargos);
        }


        /*
         * SERVICIO PARA OBTENER UN CARGO SEGÚN SU ID
         */
        [Authorize]
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Cargos>> getCargoById(int id)
        {
            Cargos cargo = _db.cargos.FirstOrDefault(c => c.id_cargo == id);

            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        /*
         * SERVICIO PARA GENERAR TOKENS VACÍOS
         */
        [HttpGet]
        public string GenerarTokenVacio()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("ASDSADAS456G6D222GsssaadsadUIOoiuiU555utjiojijijio6666ghgkjhGGHG92H56");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(),
                Issuer = "FMJ",
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }








        /*
        * SERVICIO PARA PRIMER REGISTRO DE USUARIA
        */
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<ActionResult> registrarUsuaria(datosRegistrarUsuaria datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.NickName == datos.nickname);

            if (usuaria != null)
            {
                return BadRequest($"El nombre {datos.nickname} no está disponible");
            }

            usuaria = _db.usuarias.FirstOrDefault(u => u.Email == datos.email);

            if (usuaria != null)
            {
                return BadRequest($"El email {datos.email} ya está registrado");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            datos.condiciones = true;


            if (!datos.condiciones)
            {
                return BadRequest("No se han aceptado las condiciones");
            }



            Usuarias usuariaNew = new Usuarias()
            {
                Nombre = datos.nombre,
                Apellidos = datos.apellidos,
                Email = datos.email,
                NickName = datos.nickname,
                Contra = datos.contra,
                Condiciones = datos.condiciones,


                CodPostal = 0,
                Edad = 0,
                Validado = false,
                Socia = false,
                Fk_idRol = 17,
                Registration_date = DateTime.Now,
                Modification_date = DateTime.Now,

            };

            string plantilla = metodos.ObtenerPlantillaHTMLRecordatorioSolicitud();
            string titulo = "Inicia la App";

            bool enviado = metodos.EnviarCorreoInformativo(usuariaNew.Email, usuariaNew.Nombre, titulo, plantilla);

            if (!enviado)
            {
                return BadRequest($"Error en el envío de correo a: {usuariaNew.Email}");
            }

            _db.usuarias.Add(usuariaNew);
            _db.SaveChanges();

            return Ok();
        }

        /*
         * SERVICIO PARA AÑADIR NUEVO REPOSITORIO
         */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> nuevoRepositorio(datosNuevoRepositorio datos)
        {
            Repositorio repositorio = new Repositorio
            {
                TituloArchivo = datos.TituloArchivo,
                Descripcion = datos.Descripcion,
                Estado = "ACTIVO",
                FileName = datos.FileName,
                DirigidoA = datos.DirigidoA,
                IdCategoriaArchivo = datos.IdCategoriaArchivo,
                IdArchivoRepositorio = datos.IdArchivoRepositorio,
                IdUsuaria = datos.IdUsuaria,
                Registration_Date = DateTime.Now
            };

            _db.repositorio.Add(repositorio);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR UN NUEVO ARCHIVO REPOSITORIO
         */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> nuevoArchivoRepositorio(datosNuevoArchivoRepositorio datos)
        {
            ArchivoRepositorio archivoRepositorio = new ArchivoRepositorio
            {
                ArchivoBlob = datos.ArchivoBlob,
                ContentType = datos.ContentType
            };

            _db.archivoRepositorio.Add(archivoRepositorio);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR UNA NUEVA CATEGORIA
         */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> nuevaCategoriaArchivo(datosNuevaCategoriaArchivo datos)
        {
            CategoriaArchivo nuevaCategoria = new CategoriaArchivo
            {
                Nombre = datos.Nombre,
                Descripcion = datos.Descripcion
            };

            _db.categoriaArchivo.Add(nuevaCategoria);
            _db.SaveChanges();

            return Ok();
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult>subirImagenPerfil(datosSubirImagenPerfil datos)
        {

            int idUsuaria = GetIdAdminFromToken();

            if(datos.file == null || datos.file.Length == 0 ) return BadRequest("No se envió ninguna imagen");

            using(var memoryStream = new MemoryStream())
            {
                        var foto = new Fotos()
                {

                    Foto = datos.file,
                    Fk_idUsuaria = idUsuaria,
                    Tipo = tipoFoto.PERFIL,
                    contentType = datos.contentType
                };

                _db.fotos.Add(foto);
                await
                _db.SaveChangesAsync();
                return Ok(new { foto.Id_Foto });
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> getImagenByIdUsuaria(int id)
        {

            Fotos foto = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == id && f.Tipo == tipoFoto.PERFIL);

            if (foto == null)
            {
                return NotFound();
            }

            return Ok(foto.Foto);
        }


        /*
         * SERVICIO PARA DARSE DE ALTA TRAS EL REGISTRO
         */
        [HttpPost]
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> altaUsuaria(datosAltaUsuaria datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == datos.id);

            if (usuaria == null)
            {
                return NotFound();
            }


            if (!metodos.CheckDNI(datos.dni))
            {
                return BadRequest("DNI no válido");
            }

            if (!metodos.ValidarCodigoPostal(datos.codpostal))
            {
                return BadRequest("Código postal no válido");
            }


            if (datos.validez <= DateTime.Now)
            {
                return BadRequest("Fecha del DNI inválida");
            }

            usuaria.DNI = datos.dni;
            usuaria.ValidezDNI = datos.validez;
            usuaria.FechaNacimiento = datos.fechaNac;
            usuaria.Socia = datos.socia;
            usuaria.CodPostal = datos.codpostal;
            usuaria.Estado = (int)tipoEstado.PENDIENTE;

            if (usuaria.Socia)
            {
                usuaria.Fk_IdAsociacion = datos.asociacion;
            }

            usuaria.Bio = datos.bio;

            usuaria.Referentas = JsonConvert.SerializeObject(datos.referencias);






            if (datos.intereses != null)
            {


                List<InteresesUsuarias> interesesRemove = _db.interesesUsuarias.Where(i => i.Fk_idUsuaria == datos.id).ToList();

                if (interesesRemove != null)
                {
                    foreach (var interesRemove in interesesRemove)
                    {
                        _db.interesesUsuarias.Remove(interesRemove);
                    }
                }


                for (int i = 0; i < datos.intereses.Count; i++)
                {
                    InteresesUsuarias interesesUsuarias = new InteresesUsuarias()
                    {
                        Fk_idInteres = datos.intereses[i],
                        Fk_idUsuaria = datos.id,
                    };

                    _db.interesesUsuarias.Add(interesesUsuarias);

                }
            }


            if (datos.fotoPerfil != null)
            {
                string fotoNew = datos.fotoPerfil;

                Fotos foto = new Fotos()
                {
                    Fk_idUsuaria = datos.id,
                    Tipo = tipoFoto.PERFIL,
                    Foto = fotoNew,
                    contentType = datos.fotoPerfilContentType

                };

                _db.fotos.Add(foto);
            }

            if (datos.fotoDNI != null)
            {
                string fotoNewDNI = datos.fotoDNI;

                Fotos fotoDNI = new Fotos()
                {
                    Fk_idUsuaria = datos.id,
                    Tipo = tipoFoto.DNI,
                    Foto = fotoNewDNI,
                    contentType = datos.fotoDNIContentType

                };

                _db.fotos.Add(fotoDNI);
            }

            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            string plantilla = metodos.ObtenerPlantillaHTMLRecordatorioTramitando();
            string titulo = "Alta en tramitación";
            metodos.EnviarCorreoInformativo(usuaria.Email, usuaria.Nombre, titulo, plantilla);

            return Ok();
        }





        /*
         * SERVICIO PARA AÑADIR UNA NUEV A CATEGORIA
         */
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> nuevaCategoria(CategoriaDto categoriaDto)
        {
            Categorias categoria = new Categorias
            {
                Nombre = categoriaDto.nombre,
                Descripcion = categoriaDto.descripcion,
            };

            _db.categorias.Add(categoria);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA AÑADIR NUEVAS USUARIAS
         */
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> altaUsuaria2(datosAltaUsuaria2 datos)
        {

            string errorMsg = "";


            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Email == datos.email);

            if (usuaria != null)
            {
                errorMsg = "Este email ya está siendo usado en otra cuenta";
                return BadRequest(errorMsg);
            }


            usuaria = _db.usuarias.FirstOrDefault(u => u.NickName == datos.nickname);

            if (usuaria != null)
            {
                errorMsg = "Este nickname ya está siendo utilizado";
                return BadRequest(errorMsg);
            }


            Usuarias usuariaNew = new Usuarias()
            {
                Email = datos.email,
                Nombre = datos.nombre,
                Apellidos = datos.apellidos,
                NickName = datos.nickname,
                Fk_IdAsociacion = datos.idAsociacion,
                Cargo = datos.idCargo,
                Estado = (int)tipoEstado.PENDIENTE,
                Registration_date = DateTime.Now,
                Modification_date = DateTime.Now
            };


            _db.usuarias.Add(usuariaNew);
            _db.SaveChanges();

            return Ok();

        }


        /*
         * SERVICIO APROBAR EL ALTA A UNA USUARIA POR UN ADMIN
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> solicitudAltaUsuaria(datosSolicitudAltaUsuaria datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == datos.idUsuaria);

            if (usuaria == null)
            {
                return NotFound();
            }

            Estados estado = _db.estados.FirstOrDefault(e => e.id_Estado == datos.idEstado);

            if (estado == null)
            {
                return NotFound();
            }

            if (datos.idEstado == (int)tipoEstado.VALIDADA)
            {
                var token = GenerarTokenVacio();
                usuaria.TokenRecuperacionContrasena = token;
                _db.usuarias.Update(usuaria);
                _db.SaveChanges();

                metodos.EnviarCorreoAltaAceptada(usuaria.Email, usuaria.Nombre, usuaria.TokenRecuperacionContrasena);
            }

            if (datos.idEstado == (int)tipoEstado.RECHAZADA)
            {
                usuaria.Observaciones = datos.Observaciones;
                _db.usuarias.Update(usuaria);
                _db.SaveChanges();

                metodos.EnviarCorreoAltaDenegada(usuaria.Email, usuaria.Nombre, usuaria.Observaciones);
            }

            return Ok();
        }


        


        /*
        * SERVICIO PARA VALIDAR UNA USUARIA EN EL LOGGIN
        */
        [HttpPost]     
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> login(datosLogin datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Email == datos.emailNick);
            string errorMsg = "Usuaria o contraseña incorrectos";

            if (usuaria == null)
            {
                usuaria = _db.usuarias.FirstOrDefault(u => u.NickName == datos.emailNick);

                if (usuaria == null)
                {
                    return BadRequest(errorMsg);
                }
            }



            if (usuaria.Contra != datos.contra)
            {
                return BadRequest(errorMsg);
            }



            if (usuaria.Estado == (int)tipoEstado.BLOQUEADA)
            {
                errorMsg = "POR AHORA NO TIENES PERMITIDO INICIAR SESION FAVOR DE COMUNICARTE CON UNA ADMINISTRADORA";
                return BadRequest(errorMsg);
            }


            List<int> idIntereses = _db.interesesUsuarias.Where(i => i.Fk_idUsuaria == usuaria.Id_usuaria).Select(i => i.Fk_idInteres).ToList();
            List<Intereses> intereses = _db.intereses.Where(i => idIntereses.Contains(i.Id_interes)).ToList();

            List<InteresesDto> interesesDto = new List<InteresesDto>();

            foreach (var interes in intereses)
            {
                InteresesDto interesDto = new InteresesDto
                {
                    Nombre = interes.Nombre,
                    Descripcion = interes.Descripcion,
                    Icon = interes.Icon
                };

                interesesDto.Add(interesDto);
            }



            datosGenerateJwtTokenNew usuariaDatos = new datosGenerateJwtTokenNew()
            {
                Id_usuaria = usuaria.Id_usuaria,
                Nombre = usuaria.Nombre,
                Apellidos = usuaria.Apellidos,
                NickName = usuaria.NickName,
                Email = usuaria.Email,
                TokenRecuperacionContrasena = usuaria.TokenRecuperacionContrasena,
                Edad = usuaria.Edad,
                Ubicacion = usuaria.Ubicacion,
                DNI = usuaria.DNI,
                ValidezDNI = usuaria.ValidezDNI,
                FechaNacimiento = usuaria.FechaNacimiento,
                CodPostal = usuaria.CodPostal,
                Socia = usuaria.Socia,
                Bio = usuaria.Bio,
                Registration_date = usuaria.Registration_date,
                Modification_date = usuaria.Modification_date,
                Validation_date = usuaria.Validation_date,
                Condiciones = usuaria.Condiciones,
                Validado = usuaria.Validado,
                Validation_userid = usuaria.Validation_userid,
                Referentas = usuaria.Referentas,
                Observaciones = usuaria.Observaciones,
                Fk_idRol = usuaria.Fk_idRol,
                Fk_idInsignia = usuaria.Fk_idInsignia,
                Fk_IdAsociacion = usuaria.Fk_IdAsociacion,
                Estado = usuaria.Estado,
                Cargo = usuaria.Cargo,
                intereses = interesesDto
                
            };

            var token = metodos.GenerateJwtTokenNew(usuariaDatos);

            return Ok(new { token });

        }



        [NonAction]
        private int GetIdAdminFromToken()
        {
            var token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var decodedToken = handler.ReadToken(token) as JwtSecurityToken;
            var userInfo = decodedToken.Claims.First(claim => claim.Type == "user_info").Value;
            var userInfoJson = JObject.Parse(userInfo);
            var idAdmin = int.Parse(userInfoJson["Id_usuaria"].ToString());
            return idAdmin;
        }



        /*
         * SERVICIO PARA VALIDAR USUARIA PARA USO DE ADMIN 
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> validate(int idUsuaria)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == idUsuaria);

            if (usuaria == null)
            {
                return NotFound($"Usuaria no encontrada");
            }

            usuaria.Validado = true;
            usuaria.Validation_date = DateTime.Now;
            usuaria.Validation_userid = GetIdAdminFromToken();
            usuaria.Estado = (int)tipoEstado.VALIDADA;

            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            Fotos dni = _db.fotos.FirstOrDefault(u => u.Fk_idUsuaria == idUsuaria && u.Tipo == "DNI");
            _db.fotos.Remove(dni);
            _db.SaveChanges();

            try
            {
                metodos.EnviarCorreoValidado(usuaria.Email, usuaria.Nombre);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }



            return Ok();
        }


        /*
         * SERVICIO PARA ENVIO DE CORREO SUPPORT
         */
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> correoSupport([FromBody] datosCorreoSupport datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Email == datos.email);

            if (usuaria == null)
            {
                usuaria = _db.usuarias.FirstOrDefault(u => u.NickName == datos.email);

                if (usuaria == null)
                {
                    return NotFound($"El usuario con correo/nick {datos.email} no está registrado");
                }

            }


            string token = GenerarTokenVacio();

            usuaria.TokenRecuperacionContrasena = token;
            _db.SaveChanges();

            string error = "sin error";

            try
            {
                metodos.EnviarCorreoSupport(datos.email, datos.nombreUsuaria, datos.titulo, datos.mensaje);
            }
            catch (Exception e)
            {
                error = e.Message;
                throw new Exception(error);
            }



            return Ok();
        }
       
        /*
         * SERVICIO PARA RECUPERACIÓN DE CONTRASEÑA DE USUARIA
         */

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> recuperarContrasena([FromBody] datosRecuperarContra datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Email == datos.emailNick);

            if (usuaria == null)
            {
                usuaria = _db.usuarias.FirstOrDefault(u => u.NickName == datos.emailNick);

                if (usuaria == null)
                {
                    return NotFound($"El usuario con correo/nick {datos.emailNick} no está registrado");
                }

            }


            string token = GenerarTokenVacio();

            usuaria.TokenRecuperacionContrasena = token;
            _db.SaveChanges();

            string error = "sin error";

            try
            {
                metodos.EnviarCorreoRecuperacion(usuaria.Email, usuaria.Nombre, token);
            }
            catch (Exception e)
            {
                error = e.Message;
                throw new Exception(error);
            }



            return Ok();
        }

        /*
         * SERVICIO PARA AÑADIR NUEVOS INTERESES
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult> nuevoInteres(InteresesDto datos)
        {
            Intereses interesNew = new Intereses()
            {
                Nombre = datos.Nombre,
                Descripcion = datos.Descripcion,
                Icon = datos.Icon
            };

            _db.intereses.Add(interesNew);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA AÑADIR NUEVAS INSIGNIAS
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult> nuevaInsignia(InsigniaDto datos)
        {
            Insignias insigniaNew = new Insignias()
            {
                Nombre = datos.Nombre,
                Descripcion = datos.Descripcion,
                Icon = datos.Icon
            };

            _db.insignias.Add(insigniaNew);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR NUEVO ESTADO
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> nuevoEstado(EstadosDto estado)
        {
            Estados estadoNew = new Estados
            {
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion
            };

            try
            {
                _db.estados.Add(estadoNew);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR UN NUEVO CARGO
         */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> nuevoCargo(CargosDto cargoNew)
        {
            Cargos cargo = new Cargos
            {
                Nombre = cargoNew.Nombre,
                Descripcion = cargoNew.Descripcion
            };

            _db.cargos.Add(cargo);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR NUEVA PUBLICACION
         */
        [Authorize]
        [HttpPost]
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> nuevaPublicacion(datosNuevaPublicacion datos)
        {
            int? idFoto = 0;
            int? idFotoMin = null;

            if (!string.IsNullOrEmpty(datos.foto.Foto))
            {
                var fotoNew = new Fotos
                {
                    Fk_idUsuaria = datos.idUsuaria,
                    Foto = datos.foto.Foto,
                    contentType = datos.foto.contentType,
                    Tipo = tipoFoto.PUBLICACION
                };

                _db.fotos.Add(fotoNew);
                await _db.SaveChangesAsync();

                idFoto = fotoNew.Id_Foto;
            }

            if (!string.IsNullOrEmpty(datos.fotoMin.Foto))
            {
                var fotoMinNew = new Fotos
                {
                    Fk_idUsuaria = datos.idUsuaria,
                    Foto = datos.fotoMin.Foto,
                    contentType = datos.fotoMin.contentType,
                    Tipo = tipoFoto.PUBLICACION_MIN
                };

                _db.fotos.Add(fotoMinNew);
                await _db.SaveChangesAsync();

                idFotoMin = fotoMinNew.Id_Foto;
            }

            var publicacion = new Publicaciones
            {
                fk_idFoto = idFoto,
                fk_idFotoMin = idFotoMin,
                fk_idUsuaria = datos.idUsuaria,
                Nombre = datos.nombre,
                Apellidos = datos.apellido,
                NickName = datos.nickName,
                DirigidoA = JsonConvert.SerializeObject(datos.dirigidoA),
                Insignia = datos.insignia,
                PermiteComentarios = datos.permiteComentarios,
                Visible = datos.visible,
                Titulo = datos.titulo,
                Descripcion = datos.descripcion,
                Cuerpo = datos.cuerpo,
                Ubicacion = datos.ubicacion,
                Comentarios = "",
                Reacciones = "",
                fk_idCategoria = datos.idCategoria,
                RegistrationDate = DateTime.Now,
                ModificacionDate = DateTime.Now
            };

            _db.publicaciones.Add(publicacion);
            await _db.SaveChangesAsync();

            return Ok();
        }



        /*
         * SERVICIO PARA PUBLICACION DE NUEVO COMENTARIO
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> nuevoComentario(datosNuevoComentario datos)
        {
            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == datos.idPublicacion);

            if (publicacion == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(publicacion.Comentarios))
            {
                List<Comentario> comentarios = new List<Comentario>();
                Comentario comentario = new Comentario()
                {
                    idComenatario = 1,
                    idUser = datos.comentario.idUser,
                    nombre = datos.comentario.nombre,
                    apellidos = datos.comentario.apellidos,
                    nickname = datos.comentario.nickname,
                    insignia = datos.comentario.insignia,
                    comentario = datos.comentario.comentario,
                    registrationDate = DateTime.Now,
                };

                comentarios.Add(comentario);
                publicacion.Comentarios = JsonConvert.SerializeObject(comentarios);
            }
            else
            {

                List<Comentario> comentarios = JsonConvert.DeserializeObject<List<Comentario>>(publicacion.Comentarios);
                int idComentario = comentarios.OrderByDescending(c => c.idComenatario).FirstOrDefault().idComenatario + 1;

                Comentario comentario = new Comentario
                {
                    idComenatario = idComentario,
                    idUser = datos.comentario.idUser,
                    nombre = datos.comentario.nombre,
                    apellidos = datos.comentario.apellidos,
                    nickname = datos.comentario.nickname,
                    insignia = datos.comentario.insignia,
                    comentario = datos.comentario.comentario,
                    registrationDate = DateTime.Now
                };

                comentarios.Add(comentario);
                publicacion.Comentarios = JsonConvert.SerializeObject(comentarios);
            }

            _db.publicaciones.Update(publicacion);
            _db.SaveChanges();


            return Ok();
        }


        /*
         * SERVICIO PARA AÑADIR UNA ASOCIACIÓN
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> nuevaAsociacion(AsociacionesDto asociacionDto)
        {

            //int idAsociacion = _db.asociaciones.OrderByDescending(a => a.Id_asociacion).FirstOrDefault().Id_asociacion + 1;

            Asociaciones asociacion = new Asociaciones()
            {
                //Id_asociacion = idAsociacion,
                Nombre = asociacionDto.Nombre,
                Descripcion = asociacionDto.Descripcion,
                Ubicacion = asociacionDto.Ubicacion,
                Registration_date = DateTime.Now,

            };
            _db.asociaciones.Add(asociacion);
            _db.SaveChanges();
            return Ok();
        }



        /*
         * SERVICIO PARA AÑADIR NUEVO ROL
         */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> nuevoRol(RolesDto rol)
        {
            if (rol == null)
            {
                return BadRequest();
            }

            Roles rolNew = new Roles
            {
                Nombre = rol.Nombre,
                Accesos = rol.Accesos,
                Descripcion = rol.Descripcion
            };

            _db.roles.Add(rolNew);
            _db.SaveChanges();
            return Ok();
        }



        /*
        * SERVICIO PARA INVALIDAR UNA USUARIA PASANDOLE SU ID
        */
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> invalidarUsuaria(int idUsuaria)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == idUsuaria);

            if (usuaria == null)
            {
                return NotFound();
            }

            usuaria.Validado = false;
            usuaria.Estado = (int)tipoEstado.RECHAZADA;

            List<Publicaciones> publicaciones = _db.publicaciones.Where(p => p.fk_idUsuaria == idUsuaria).ToList();

            if (publicaciones != null)
            {
                foreach (var publicacion in publicaciones)
                {
                    _db.publicaciones.Remove(publicacion);
                }

                _db.SaveChanges();
            }

            List<Fotos> fotos = _db.fotos.Where(f => f.Fk_idUsuaria == idUsuaria).ToList();

            if (fotos != null)
            {
                foreach (var foto in fotos)
                {
                    _db.fotos.Remove(foto);
                }

                _db.SaveChanges();
            }

            return Ok();
        }



        /*
         * SERVICIO PARA CAMBIAR ESTADO DE USUARIA
         */

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> cambiarEstadoUsuaria(datosCambiarEstadoUsuaria datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == datos.idUsuaria);

            if (usuaria == null)
            {
                return NotFound();
            }

            Estados estado = _db.estados.FirstOrDefault(e => e.id_Estado == datos.idEstado);

            if (estado == null)
            {
                return NotFound();
            }

            if (estado.id_Estado == (int)tipoEstado.PENDIENTE)
            {
                usuaria.Validado = false;
            }

            if (estado.id_Estado == (int)tipoEstado.VALIDADA)
            {
                usuaria.Validado = true;
            }
            usuaria.Estado = datos.idEstado;

            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            return Ok();
        }



        [Authorize]
        [HttpPost]
        public async Task<ActionResult> caducidadUsuarias()
        {

            DateTime todayDate = DateTime.Now;
            DateTime registrationDate = new DateTime();
            int diasDiferencia = 0;
            int diasRestantes = 0;

            string tituloCorreo = "";
            string plantilla = "";

            List<Usuarias> usuarias = _db.usuarias.ToList();

            foreach (var usuaria in usuarias)
            {

                if (usuaria.DNI == null || usuaria.DNI.Trim() == "")
                {

                    registrationDate = usuaria.Registration_date;

                    TimeSpan diferencia = todayDate.Subtract(registrationDate);
                    diasDiferencia = diferencia.Days;


                    if (diasDiferencia >= 25 && diasDiferencia < 30)
                    {
                        tituloCorreo = "Recordatorio: Verifique su identidad";
                        diasRestantes = 30 - diasDiferencia;
                        plantilla = metodos.ObtenerPlantillaHTMLRecordatorioIdentidadNoVerificada(diasRestantes.ToString());

                        metodos.EnviarCorreoInformativo(usuaria.Email, usuaria.Nombre, tituloCorreo, plantilla);
                    }

                    if (diasDiferencia >= 30)
                    {
                        tituloCorreo = "Su cuenta ha sido eliminada";
                        plantilla = metodos.ObtenerPlantillaHTMLEliminacionCuenta();
                        deleteUsuaria(usuaria.Id_usuaria);

                        metodos.EnviarCorreoInformativo(usuaria.Email, usuaria.Nombre, tituloCorreo, plantilla);
                    }
                }

            }

            return Ok();
        }




        /*
         * SERVICIO PARA ELIMINAR UN CARGO PASANDOLE SU ID
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> eliminarCargo(int id)
        {
            Cargos cargo = _db.cargos.FirstOrDefault(c => c.id_cargo == id);

            if (cargo == null)
            {
                return NotFound();
            }

            try
            {
                _db.cargos.Remove(cargo);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok();
        }



        /*
         * SERVICIO PARA ELIMINAR UN ESTADO
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> eliminarEstado(int id)
        {
            Estados estado = _db.estados.FirstOrDefault(e => e.id_Estado == id);

            if (estado == null)
            {
                return NotFound();
            }

            try
            {
                _db.estados.Remove(estado);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        /*
         * SERVICIO PARA ELIMINAR UN COMENTARIO
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> eliminarComentario(datosEliminarComentario datos)
        {

            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == datos.idPublicacion);

            if (publicacion == null)
            {
                return NotFound();
            }

            List<Comentario> comentarios = JsonConvert.DeserializeObject<List<Comentario>>(publicacion.Comentarios);
            Comentario comentario = comentarios.FirstOrDefault(c => c.idComenatario == datos.idComentario);
            comentarios.Remove(comentario);

            string comentariosActualziados = JsonConvert.SerializeObject(comentarios);
            publicacion.Comentarios = comentariosActualziados;

            _db.publicaciones.Update(publicacion);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ELIMINAR UNA PUBLICACION MEDIANTE SU ID
         */
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> eliminarPublicacion(int id)
        {
            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == id);

            if (publicacion == null)
            {
                return NotFound();
            }

            Fotos fotoPubli = _db.fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);

            if (fotoPubli != null)
            {
                _db.fotos.Remove(fotoPubli);
            }

            _db.publicaciones.Remove(publicacion);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ELIMINAR UN ASOCIACION MEDIANTE SU ID
         */
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> eliminarAsociacion(int id)
        {

            Asociaciones asociacion = _db.asociaciones.FirstOrDefault(a => a.Id_asociacion == id);

            if (asociacion == null)
            {
                return NotFound();
            }

            _db.asociaciones.Remove(asociacion);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ELIMINAR UN INTERES MEDIANTE SU ID
         */
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> eliminarInteres(int id)
        {
            Intereses interes = _db.intereses.FirstOrDefault(i => i.Id_interes == id);

            if (interes == null)
            {
                return NotFound();
            }

            _db.intereses.Remove(interes);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ELIMIANR REPOSITORIO POR SU ID
         */
        [Authorize]
        [HttpDelete]

        public async Task<ActionResult> eliminarRepositorio(int id)
        {
            Repositorio repositorio = _db.repositorio.FirstOrDefault(r => r.IdRepositorio == id);

            if (repositorio == null)
            {
                return NotFound("No existe repositorio con este id");
            }

            _db.repositorio.Remove(repositorio);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA ELIMINAR ARCHIVO REPOSITORIO
         */
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> eliminarArchivoRepositorio(int id)
        {
            ArchivoRepositorio archivo = _db.archivoRepositorio.FirstOrDefault(a => a.IdArchivoRepositorio == id);

            if (archivo == null)
            {
                return NotFound("No existe archivo con ese id");
            }

            _db.archivoRepositorio.Remove(archivo);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA ELIMINAR UNA CATEGORIA ARCHIVO
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> eliminarCategoriaArchivo(int id)
        {
            CategoriaArchivo categoria = _db.categoriaArchivo.FirstOrDefault(c => c.IdCategoriaArchivo == id);

            if (categoria == null)
            {
                return NotFound("No existe categoria con ese id");
            }

            _db.categoriaArchivo.Remove(categoria);
            _db.SaveChanges();

            return Ok();
        }

        /*
         * SERVICIO PARA ELIMINAR UN INSIGNIAS MEDIANTE SU ID
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> eliminarInsignia(int id)
        {
            Insignias insignia = _db.insignias.FirstOrDefault(i => i.Id_insignia == id);

            if (insignia == null)
            {
                return NotFound();
            }

            _db.insignias.Remove(insignia);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA ELIMINAR UNA USUARIA DE LA BASE DE DATOS PASÁNDOLE SU ID
         */
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> deleteUsuaria(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Usuarias userDB = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == id);

            if (userDB == null)
            {
                return NotFound();
            }
            else
            {


                Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.fk_idUsuaria == id);

                while (publicacion != null)
                {
                    _db.publicaciones.Remove(publicacion);
                    _db.SaveChanges();
                    publicacion = _db.publicaciones.FirstOrDefault(p => p.fk_idUsuaria == id);
                }

                Fotos Foto = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == id);

                while (Foto != null)
                {
                    _db.fotos.Remove(Foto);
                    _db.SaveChanges();
                    Foto = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == id);
                }

                _db.usuarias.Remove(userDB);
                _db.SaveChanges();
            }

            return Ok();
        }


        /*
         * SERVICIO PARA ELIMINAR UN ROL
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> eliminarRol(int id)
        {

            Roles rol = _db.roles.FirstOrDefault(r => r.Id_rol == id);

            if (rol == null)
            {
                return NotFound();
            }

            try
            {
                _db.roles.Remove(rol);
                _db.SaveChanges();
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }


            return Ok();
        }

        /*
         * SERVICIO PARA ELIMINAR UNA CATEGORIA
         */
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> eliminarCategoria(int id)
        {
            Categorias categoria = _db.categorias.FirstOrDefault(c => c.Id_Categoria == id);

            if (categoria == null)
            {
                return NotFound("No existe categoria con ese id");
            }

            _db.categorias.Remove(categoria);
            _db.SaveChanges();

            return Ok();
        }

        /*
         * SERVICIO PARA ACTUALIZAR UN REPOSITORIO
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> updateRepositorio(datosUpdateRepositorio datos)
        {
            Repositorio repositorio = _db.repositorio.FirstOrDefault(r => r.IdRepositorio == datos.IdRepositorio);

            if (repositorio == null)
            {
                return NotFound("No existe repositorio con ese id");
            }

            repositorio.TituloArchivo = datos.TituloArchivo;
            repositorio.DirigidoA = datos.DirigidoA;
            repositorio.IdArchivoRepositorio = datos.IdArchivoRepositorio;
            repositorio.IdCategoriaArchivo = datos.IdCategoriaArchivo;
            repositorio.Descripcion = datos.Descripcion;
            repositorio.FileName = datos.FileName;
            repositorio.IdUsuaria = datos.IdUsuaria;


            _db.repositorio.Update(repositorio);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA ACTUALIZAR ARCHIVO REPOSITORIO
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> updateArchivoRepositorio(datosUpdateArchivoRepositorio datos)
        {
            ArchivoRepositorio archivo = _db.archivoRepositorio.FirstOrDefault(a => a.IdArchivoRepositorio == datos.IdArchivoRepositorio);

            if (archivo == null)
            {
                return NotFound();
            }

            archivo.ArchivoBlob = datos.ArchivoBlob;
            archivo.ContentType = datos.ContentType;

            _db.archivoRepositorio.Update(archivo);
            _db.SaveChanges();

            return Ok();
        }




        /*
         * SERVICIO PARA ACTUALIZAR UNA CATEGORIA ARCHIVO
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> updateCategoriaArchivo(datosUpdateCategoriaArchivo datos)
        {
            CategoriaArchivo categoria = _db.categoriaArchivo.FirstOrDefault(c => c.IdCategoriaArchivo == datos.IdCategoriaArchivo);

            if (categoria == null)
            {
                return NotFound("No existe categoria con ese id");
            }

            categoria.Nombre = datos.Nombre;
            categoria.Descripcion = datos.Descripcion;

            _db.categoriaArchivo.Update(categoria);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ACTUALIZAR UNA CATEGORIA
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> updateCategoria(Categorias categoria)
        {
            Categorias categoriaUpt = _db.categorias.FirstOrDefault(c => c.Id_Categoria == categoria.Id_Categoria);

            if (categoriaUpt == null)
            {
                return NotFound("No existe categoria con ese id");
            }

            categoriaUpt.Nombre = categoria.Nombre;
            categoriaUpt.Descripcion = categoria.Descripcion;

            _db.categorias.Update(categoriaUpt);
            _db.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> updateRol(datosUpdateRoles datos)
        {
            Roles rolUpd = _db.roles.FirstOrDefault(r => r.Id_rol == datos.idRol);

            if (rolUpd == null)
            {
                return NotFound();
            }

            rolUpd.Nombre = datos.rol.Nombre;
            rolUpd.Accesos = datos.rol.Accesos;
            rolUpd.Descripcion = datos.rol.Descripcion;

            try
            {
                _db.roles.Update(rolUpd);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok();
        }


        /*
         * SERVICIO PARA ACTUALIZAR UNA ASOCIACION MEDIANTE SU ID
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updateAsociacion(datosUpdateAsociacion datos)
        {
            Asociaciones asociacion = _db.asociaciones.FirstOrDefault(a => a.Id_asociacion == datos.id);

            if (asociacion == null)
            {
                return NotFound();
            }

            asociacion.Nombre = datos.asociacionDto.Nombre;
            asociacion.Descripcion = datos.asociacionDto.Descripcion;
            asociacion.Ubicacion = asociacion.Ubicacion;

            _db.asociaciones.Update(asociacion);
            _db.SaveChanges();

            return Ok();
        }


        /*
        * SERVICIO PARA ACTUALIZAR UN INTERES MEDIANTE SU ID
        */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updateInteres(datosUpdateInteres datos)
        {
            Intereses interes = _db.intereses.FirstOrDefault(i => i.Id_interes == datos.id);

            if (interes == null)
            {
                return NotFound();
            }

            interes.Nombre = datos.interesDto.Nombre;
            interes.Descripcion = datos.interesDto.Descripcion;
            interes.Icon = datos.interesDto.Icon;


            _db.intereses.Update(interes);
            _db.SaveChanges();

            return Ok();
        }


        /*
       * SERVICIO PARA ACTUALIZAR UN INSIGNIAS MEDIANTE SU ID
       */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updateInsignia(datosUpdateInsignia datos)
        {
            Insignias insignia = _db.insignias.FirstOrDefault(i => i.Id_insignia == datos.id);

            if (insignia == null)
            {
                return NotFound();
            }

            insignia.Nombre = datos.insigniaDto.Nombre;
            insignia.Descripcion = datos.insigniaDto.Descripcion;
            insignia.Icon = datos.insigniaDto.Icon;


            _db.insignias.Update(insignia);
            _db.SaveChanges();

            return Ok();
        }

        /*
        * SERVICIO PARA ACTUALIZAR UNA PUBLICACION MEDIANTE SU ID
        */
        [Authorize]
        [HttpPut]
        [RequestSizeLimit(52428800)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updatePublicacion(datosUpdatePublicacion datos)
        {
            Publicaciones publicacion = _db.publicaciones.FirstOrDefault(p => p.Id_publicacion == datos.id);

            if (publicacion == null)
            {
                return NotFound();
            }

            publicacion.ModificacionDate = DateTime.Now;
            publicacion.Descripcion = datos.publicacionDto.Descripcion;
            publicacion.Cuerpo = datos.publicacionDto.Cuerpo;
            publicacion.Titulo = datos.publicacionDto.Titulo;
            publicacion.Ubicacion = datos.publicacionDto.Ubicacion;
            publicacion.Nombre = datos.publicacionDto.Nombre;
            publicacion.Apellidos = datos.publicacionDto.Apellidos;
            publicacion.NickName = datos.publicacionDto.NickName;
            publicacion.DirigidoA = JsonConvert.SerializeObject(datos.publicacionDto.DirigidoA);
            publicacion.PermiteComentarios = datos.publicacionDto.PermiteComentarios;
            publicacion.fk_idCategoria = datos.publicacionDto.fk_idCategoria;
            publicacion.Visible = datos.publicacionDto.Visible;


            Fotos foto = _db.fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFoto);

            if (foto != null)
            {
                string fotoNew = datos.fotoNueva.Foto;
                string contentType = datos.fotoNueva.contentType;
                foto.Foto = fotoNew;
                foto.contentType = contentType;
                _db.fotos.Update(foto);
            }

            Fotos fotoMin = _db.fotos.FirstOrDefault(f => f.Id_Foto == publicacion.fk_idFotoMin);

            if (fotoMin != null)
            {
                string fotoMinNew = datos.fotoMinNueva.Foto;
                string contentTypeMin = datos.fotoMinNueva.contentType;
                fotoMin.Foto = fotoMinNew;
                fotoMin.contentType = contentTypeMin;
                _db.fotos.Update(fotoMin);
            }


            _db.publicaciones.Update(publicacion);
            _db.SaveChanges();

            return Ok();
        }

        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpPut]
        public async Task<ActionResult> updateEstado(datosUpdateEstado datos)
        {
            Estados estadoUpd = _db.estados.FirstOrDefault(e => e.id_Estado == datos.idEstado);

            if (estadoUpd == null)
            {
                return NotFound();
            }

            estadoUpd.Nombre = datos.estado.Nombre;
            estadoUpd.Descripcion = datos.estado.Descripcion;

            _db.estados.Update(estadoUpd);
            _db.SaveChanges();

            return Ok();
        }


        /*
         * SERVICIO PARA ACTUALIZAR UN CARGO
         */
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpPut]
        public async Task<ActionResult> updateCargo(Cargos cargoDatos)
        {
            Cargos cargoUpdate = _db.cargos.FirstOrDefault(c => c.id_cargo == cargoDatos.id_cargo);

            if (cargoUpdate == null)
            {
                return NotFound();
            }

            cargoUpdate.Nombre = cargoDatos.Nombre;
            cargoUpdate.Descripcion = cargoDatos.Descripcion;


            _db.cargos.Update(cargoUpdate);
            _db.SaveChanges();

            return Ok();
        }




        /*
         * SERVICIO PARA ACTUALIZAR EL ROL DE UNA USUARIA
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updateRolUsuaria(datosUpdateRolUsuaria datos)
        {

            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == datos.idUsuaria);         

            int idUsuariaValidate = GetIdAdminFromToken();

            usuaria.Fk_idRol = datos.idRol;
            usuaria.Validation_userid = idUsuariaValidate;

            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ACTUALIZAR OBSERVACIONES DE UNA USUARIA
         */
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updateObservacionesUsuaria(datosUpdateObservacionesUsuaria datos)
        {
            Usuarias usuaria = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == datos.idUsuaria);

            if (usuaria == null)
            {
                return NotFound();
            }

            usuaria.Observaciones = datos.Observaciones;

            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            return Ok();
        }

        /*
        * SERVICIO PARA ACTUALIZAR TODOS LOS DATOS DE UNA USUARIA
        */
        //[Authorize]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> editUser(UsuariasDto usuaria)
        {


            try
            {
                Usuarias usuariaUpdate = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == usuaria.Id_usuaria);

                if (usuariaUpdate == null || !usuariaUpdate.Validado)
                {
                    return BadRequest();
                }


                if (usuaria.fotoPerfil != null && usuaria.fotoPerfil != "")
                {

                    Fotos fotoUpdate = _db.fotos.FirstOrDefault(f => f.Fk_idUsuaria == usuaria.Id_usuaria && f.Tipo == tipoFoto.PERFIL);

                    if (fotoUpdate != null)
                    {
                        fotoUpdate.Foto = usuaria.fotoPerfil;
                        fotoUpdate.contentType = usuaria.fotoPerfilContentType;
                        _db.fotos.Update(fotoUpdate);
                    }
                    else
                    {
                        Fotos foto = new Fotos()
                        {
                            Fk_idUsuaria = usuaria.Id_usuaria,
                            Tipo = tipoFoto.PERFIL,
                            Foto = usuaria.fotoPerfil,
                            contentType = usuaria.fotoPerfilContentType

                        };

                        _db.fotos.Add(foto);
                    }

                    _db.SaveChanges();
                   
                }

                establecerDatosUsuaria(usuaria, usuariaUpdate);

                _db.usuarias.Update(usuariaUpdate);

                actualizarIntereses(usuaria);

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        private void actualizarIntereses(UsuariasDto usuaria)
        {
            List<InteresesUsuarias> interesesFind = _db.interesesUsuarias.Where(i => i.Fk_idUsuaria == usuaria.Id_usuaria).ToList();

            if (interesesFind != null)
            {
                foreach (var interes in interesesFind)
                {
                    _db.interesesUsuarias.Remove(interes);
                }
            }


            if (usuaria.interesesID != null && usuaria.interesesID.Count > 0)
            {
                for (int i = 0; i < usuaria.interesesID.Count; i++)
                {
                    InteresesUsuarias interesNew = new InteresesUsuarias
                    {
                        Fk_idInteres = usuaria.interesesID[i],
                        Fk_idUsuaria = usuaria.Id_usuaria
                    };

                    _db.interesesUsuarias.Add(interesNew);

                }
            }
        }

        private static void establecerDatosUsuaria(UsuariasDto usuaria, Usuarias usuariaUpdate)
        {
            usuariaUpdate.Nombre = usuaria.Nombre;
            usuariaUpdate.Apellidos = usuaria.Apellidos;
            usuariaUpdate.NickName = usuaria.NickName;
            usuariaUpdate.Email = usuaria.Email;
            usuariaUpdate.Contra = usuaria.Contra;
            usuariaUpdate.Edad = usuaria.Edad;
            usuariaUpdate.DNI = usuaria.DNI;
            usuariaUpdate.ValidezDNI = usuaria.ValidezDNI;
            usuariaUpdate.CodPostal = usuaria.CodPostal;
            usuariaUpdate.Bio = usuaria.Bio;
            usuariaUpdate.Referentas = JsonConvert.SerializeObject(usuaria.Referentas);
            usuariaUpdate.FechaNacimiento = usuaria.FechaNacimiento;
            usuariaUpdate.Socia = usuaria.Socia;
            usuariaUpdate.Fk_IdAsociacion = usuaria.Fk_IdAsociacion;
            usuariaUpdate.Fk_idInsignia = usuaria.Fk_idInsignia;
            usuariaUpdate.Fk_idRol = usuaria.Fk_idRol;
            usuariaUpdate.Ubicacion = JsonConvert.SerializeObject(usuaria.Ubicacion);
            usuariaUpdate.Modification_date = DateTime.Now;
        }






        /* 
         * SERVICIO PARA ACTUALIZAR LA CONTRASEÑA
         */
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> updatePassword(datosUpdatePassword datos)
        {
            var usuaria = _db.usuarias.FirstOrDefault(u => u.TokenRecuperacionContrasena == datos.token);

            if (usuaria == null)
            {
                return NotFound();
            }

            usuaria.Contra = datos.nuevaContrasena;
            _db.usuarias.Update(usuaria);
            _db.SaveChanges();

            return Ok();
        }



        /*
         * SERVICIO PARA ACTUALIZAR SOLAMENTE UN DATO DE UNA USUARIA PASANDOLE SU ID
         */
        [Authorize]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UsuariasDto>> updateUserDate(int id, JsonPatchDocument<Usuarias> datos)
        {
            if (datos == null || id == 0)
            {
                return BadRequest();
            }

            Usuarias usuariaUpdate = _db.usuarias.FirstOrDefault(u => u.Id_usuaria == id);

            if (usuariaUpdate == null)
            {
                return NotFound();
            }

            datos.ApplyTo(usuariaUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.usuarias.Update(usuariaUpdate);
            _db.SaveChanges();


            return NoContent();




            //EJEMPLO DE LOS DATOS A CUBRIR PARA ACTUALIZAR EL DATO DE UNA COLUMNA

            /*
             *  "path": "/nombre",
                "op": "replace",
                "value": "PRUEBAACTUALIZADA"
             */

        }

    }
}
