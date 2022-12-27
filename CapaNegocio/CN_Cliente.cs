using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    { 
    
        private CD_Cliente Objeto_CapaDato = new CD_Cliente();


        public int Registrar(Cliente objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Nombres) || string.IsNullOrWhiteSpace(objeto.Nombres))
            {
                Mensaje = "El Nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Apellidos) || string.IsNullOrWhiteSpace(objeto.Apellidos))
            {
                Mensaje = "El Apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Correo))
            {
                Mensaje = "El Correo del cliente no puede ser vacio";
            }

            //Aqui validamos si el mensaje sigue vacio. Si ese es el caso entonces no hubo errores.
            if (string.IsNullOrEmpty(Mensaje))
            {
                objeto.Clave = CN_Recursos.ConvertiSha256(objeto.Clave);

                return Objeto_CapaDato.Registrar(objeto, out Mensaje);
            }
            else
            {
                //Retornamos el valor de 0 para saber que no se ha creado un usuario.
                return 0;
            }
        }

        public List<Cliente> Listar()
        {
            //Ocupo metodo Listar de CD_Cliente.
            return Objeto_CapaDato.Listar();
        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {

            return Objeto_CapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = Objeto_CapaDato.ReestablecerClave(idcliente, CN_Recursos.ConvertiSha256(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensajeCorreo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se puedo enviar el Correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la Contrasena";
                return false;
            }
        }
    }
}
