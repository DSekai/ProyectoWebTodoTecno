using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("ManGasTesting@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("ManGasTesting@gmail.com", "iiznsezhurhmfoqi"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch(Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }

        public static string ConvertiSha256(string texto) 
        {
            //Encriptacion de texto en SHA256
            StringBuilder sb = new StringBuilder();
            //Usar la referencia de using "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create()) 
            {

                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result) 
                {
                    sb.Append(b.ToString("x2"));          
                }
                return sb.ToString();                       
            }             
        }

        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                // La ruta se convierte a un array del tipo byte
                byte[] bytes = File.ReadAllBytes(ruta);
                // Se convierte a Base64 el array
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
