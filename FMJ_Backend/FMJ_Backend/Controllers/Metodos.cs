using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FMJ_Backend.Controllers
{
    public class Metodos
    {

        private readonly ServicioCorreoConfig _configuracionCorreo;

        public Metodos(ServicioCorreoConfig configuracionCorreo)
        {
            _configuracionCorreo = configuracionCorreo;
        }


        public string GenerateJwtTokenNew(datosGenerateJwtTokenNew usuaria)
        {


            string jsonUsuaria = JsonConvert.SerializeObject(usuaria);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("user_info", jsonUsuaria)

                }),
                Issuer = "FMJ",
                Expires = DateTime.UtcNow.AddYears(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;


        }





        //<-------------------------------OBTENCIÓN DE PLANTILLAS HTML---------------------------------------------------------->



        private string ObtenerPlantillaHTML(string token)
        {
            // Lee el contenido del archivo que contiene la plantilla HTML

            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "recuperarContrasena.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);

            contenidoPlantilla = contenidoPlantilla.Replace("{{token}}", token);
            contenidoPlantilla = contenidoPlantilla.Replace("{{URL}}", "https://fmj-mobile.azurewebsites.net/changePass");
            //contenidoPlantilla = contenidoPlantilla.Replace("{{URL}}", "http://localhost:4200/changePass");
            return contenidoPlantilla;
        }

        private string ObtenerPlantillaHTMLValidado()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "cuentaValidada.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }
        private string ObtenerPlantillaSupport()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "emailSupport.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }


        private  string ObtenerPlantillaHTMLAltaAprobada(string token)
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "altaAprobada.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);


            contenidoPlantilla = contenidoPlantilla.Replace("{{token}}", token);
            contenidoPlantilla = contenidoPlantilla.Replace("{{URL}}", "https://fmj-mobile.azurewebsites.net/changePass");
            return contenidoPlantilla;
        }


        public string ObtenerPlantillaHTMLAltaDenegada()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "altaDenegada.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }


        public string ObtenerPlantillaHTMLRecordatorioSolicitud()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "recordatorioSolicitud.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }



        public string ObtenerPlantillaHTMLRecordatorioTramitando()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "recordatorioTramitando.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }


        public string ObtenerPlantillaHTMLRecordatorioIdentidadNoVerificada(string dias)
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "recordatorioIdentidadNoVerficada.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);

            contenidoPlantilla = contenidoPlantilla.Replace("{{dias}}", dias);
            return contenidoPlantilla;
        }


        public string ObtenerPlantillaHTMLEliminacionCuenta()
        {
            string rutaArchivoPlantilla = Path.Combine("HTML", "templates", "recordatorioEliminacion.html");
            string contenidoPlantilla = System.IO.File.ReadAllText(rutaArchivoPlantilla);
            return contenidoPlantilla;
        }



        //<-----------------------------------------ENVÍO DE CORREOS----------------------------------------------------->


        public void EnviarCorreoRecuperacion(string email, string nombreUsuaria, string token)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("fmj@acha.es");
                mail.To.Add(email);
                mail.Subject = "Recuperación de Contraseña";


                string htmlBody = ObtenerPlantillaHTML(token);


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenCandado = Path.Combine("HTML", "images", "image-1.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenCandado = new Attachment(rutaImagenCandado);
                imagenCandado.ContentId = "imagenCandado";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenCandado);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenCandado}}", "cid:imagenCandado");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");
                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");



                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                client.Send(mail);
            }
        }


        [NonAction]
        // MÉTODO PARA ENVIAR EL CORREO ELECTRÓNICO
        public void EnviarCorreoValidado(string email, string nombreUsuaria)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("fmj@acha.es");
                mail.To.Add(email);
                mail.Subject = "Cuenta Validada";


                string htmlBody = ObtenerPlantillaHTMLValidado();


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenVerificado = Path.Combine("HTML", "images", "checkblanco.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenVerificado = new Attachment(rutaImagenVerificado);
                imagenVerificado.ContentId = "imagenVerificado";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenVerificado);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenVerificado}}", "cid:imagenVerificado");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");
                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");


                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                client.Send(mail);
            }
        }



        [NonAction]
        // MÉTODO PARA ENVIAR EL CORREO ELECTRÓNICO
        public void EnviarCorreoAltaAceptada(string email, string nombreUsuaria, string token)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("fmj@acha.es");
                mail.To.Add(email);
                mail.Subject = "Alta Aprobada";


                string htmlBody = ObtenerPlantillaHTMLAltaAprobada(token);


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenVerificado = Path.Combine("HTML", "images", "checkblanco.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenVerificado = new Attachment(rutaImagenVerificado);
                imagenVerificado.ContentId = "imagenVerificado";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenVerificado);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenVerificado}}", "cid:imagenVerificado");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");
                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");


                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                client.Send(mail);
            }
        }



        [NonAction]
        // MÉTODO PARA ENVIAR EL CORREO ELECTRÓNICO
        public void EnviarCorreoAltaDenegada(string email, string nombreUsuaria, string motivoRechazo)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("fmj@acha.es");
                mail.To.Add(email);
                mail.Subject = "Alta Denegada";


                string htmlBody = ObtenerPlantillaHTMLAltaDenegada();


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenDenegado = Path.Combine("HTML", "images", "denegado.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenDenegado = new Attachment(rutaImagenDenegado);
                imagenDenegado.ContentId = "imagenDenegado";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenDenegado);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{motivoRechazo}}", motivoRechazo);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenDenegado}}", "cid:imagenDenegado");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");

                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");


                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                client.Send(mail);
            }
        }

        [NonAction]
        // MÉTODO PARA ENVIAR EL CORREO ELECTRÓNICO
        public bool EnviarCorreoInformativo(string email, string nombreUsuaria, string titulo, string plantilla)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("fmj@acha.es");
                mail.To.Add(email);
                mail.Subject = titulo;


                //string htmlBody = ObtenerPlantillaHTMLRecordatorioSolicitud();
                string htmlBody = plantilla;


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenInformativo = Path.Combine("HTML", "images", "informativo.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenInformativo = new Attachment(rutaImagenInformativo);
                imagenInformativo.ContentId = "imagenInformativo";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenInformativo);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenInformativo}}", "cid:imagenInformativo");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");

                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");


                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                try
                {
                    client.Send(mail);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }


        // MÉTODO PARA ENVIAR EL CORREO ELECTRÓNICO POR PARTE DE UNA USUARIA
        public void EnviarCorreoSupport(string email, string nombreUsuaria, string titulo, string mensaje)
        {


            var servidor = _configuracionCorreo.Servidor;
            var puerto = _configuracionCorreo.Puerto;
            var usuario = _configuracionCorreo.Usuario;
            var contraseña = _configuracionCorreo.Contra;


            using (SmtpClient client = new SmtpClient(servidor, puerto))
            {


                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(usuario, contraseña);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                mail.To.Add("fmj@acha.es");
                mail.Subject = titulo;


                //string htmlBody = ObtenerPlantillaHTMLRecordatorioSolicitud();
                string htmlBody = this.ObtenerPlantillaSupport();


                string rutaImagenFMJ = Path.Combine("HTML", "images", "fmjrosapng.png");
                string rutaImagenInformativo = Path.Combine("HTML", "images", "informativo.png");
                string rutaImagenFace = Path.Combine("HTML", "images", "image-2.png");
                string rutaImagenInsta = Path.Combine("HTML", "images", "image-3.png");
                string rutaImagenX = Path.Combine("HTML", "images", "image-4.png");
                //string rutaImagenLinkdn = Path.Combine("HTML", "images", "image-5.png");
                string rutaImagenYoutube = Path.Combine("HTML", "images", "youtube.png");

                Attachment imagenFMJ = new Attachment(rutaImagenFMJ);
                imagenFMJ.ContentId = "imagenFMJ";

                Attachment imagenInformativo = new Attachment(rutaImagenInformativo);
                imagenInformativo.ContentId = "imagenInformativo";

                Attachment imagenFace = new Attachment(rutaImagenFace);
                imagenFace.ContentId = "imagenFace";

                Attachment imagenInsta = new Attachment(rutaImagenInsta);
                imagenInsta.ContentId = "imagenInsta";

                Attachment imagenX = new Attachment(rutaImagenX);
                imagenX.ContentId = "imagenX";

                //Attachment imagenLinkdn = new Attachment(rutaImagenLinkdn);
                //imagenLinkdn.ContentId = "imagenLinkdn";

                Attachment imagenYoutube = new Attachment(rutaImagenYoutube);
                imagenYoutube.ContentId = "imagenYoutube";

                mail.Attachments.Add(imagenFMJ);
                mail.Attachments.Add(imagenInformativo);
                mail.Attachments.Add(imagenX);
                mail.Attachments.Add(imagenFace);
                mail.Attachments.Add(imagenInsta);
                // mail.Attachments.Add(imagenLinkdn);
                mail.Attachments.Add(imagenYoutube);

                // Reemplazar la referencia a la imagen en el HTML
                htmlBody = htmlBody.Replace("{{nombreUsuaria}}", nombreUsuaria);
                htmlBody = htmlBody.Replace("{{contenido}}", mensaje);
                htmlBody = htmlBody.Replace("{{imagenFMJ}}", "cid:imagenFMJ");
                htmlBody = htmlBody.Replace("{{imagenInformativo}}", "cid:imagenInformativo");
                htmlBody = htmlBody.Replace("{{imagenFace}}", "cid:imagenFace");
                htmlBody = htmlBody.Replace("{{imagenInsta}}", "cid:imagenInsta");
                htmlBody = htmlBody.Replace("{{imagenX}}", "cid:imagenX");
                htmlBody = htmlBody.Replace("{{imagenYoutube}}", "cid:imagenYoutube");

                //htmlBody = htmlBody.Replace("{{imagenLinkdn}}", "cid:imagenLinkdn");


                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                client.Send(mail);
            }

        }


        //<---------------------------------VALIDACIONES---------------------------------------------->



        public bool CheckDNI(string dni)
        {
            // Comprobamos si el DNI tiene 9 dígitos
            if (dni.Length != 9)
            {
                // No es un DNI válido
                return false;
            }

            // Extraemos los números y la letra
            string dniNumbers = dni.Substring(0, dni.Length - 1);
            string dniLetter = dni.Substring(dni.Length - 1, 1);

            // Intentamos convertir los números del DNI a integer
            var numbersValid = int.TryParse(dniNumbers, out int dniInteger);
            if (!numbersValid)
            {
                // No se pudo convertir los números a formato numérico
                return false;
            }

            if (CalculateDNILetter(dniInteger) != dniLetter)
            {
                // La letra del DNI es incorrecta
                return false;
            }

            // DNI válido :)
            return true;
        }
      
        public string CalculateDNILetter(int dniNumbers)
        {
            // Cargamos los dígitos de control
            string[] control = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
            var mod = dniNumbers % 23;
            return control[mod];
        }



        public bool ValidarCodigoPostal(int codigoPostal)
        {
            // Comprobamos si el código postal tiene 5 dígitos
            if (codigoPostal.ToString().Length != 5)
            {
                // No es un código postal válido
                return false;
            }

            // Código postal válido :)
            return true;
        }
    }
}
