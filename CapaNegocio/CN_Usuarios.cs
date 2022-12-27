using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        //Instancio clase CD_Usuarios para ocupar sus metodos.
        private CD_Usuarios Objeto_CapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            //Ocupo metodo Listar de CD_Usuarios.
            return Objeto_CapaDato.Listar();
        }

        public int Registrar(Usuario objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Nombres) || string.IsNullOrWhiteSpace(objeto.Nombres))
            {
                Mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Apellidos) || string.IsNullOrWhiteSpace(objeto.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            //Aqui validamos si el mensaje sigue vacio. Si ese es el caso entonces no hubo errores.
            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de cuenta :3";

                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(objeto.Correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    objeto.Clave = CN_Recursos.ConvertiSha256(clave);

                    return Objeto_CapaDato.Registrar(objeto, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";

                    return 0;
                }

                //Se esta forma va a encriptar la clave
                objeto.Clave = CN_Recursos.ConvertiSha256(clave);

                return Objeto_CapaDato.Registrar(objeto, out Mensaje);
            }
            else
            {
                //Retornamos el valor de 0 para saber que no se ha creado un usuario.
                return 0;
            }
        }

        public bool Editar(Usuario objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Nombres) || string.IsNullOrWhiteSpace(objeto.Nombres))
            {
                Mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Apellidos) || string.IsNullOrWhiteSpace(objeto.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            //Aqui validamos si el mensaje sigue vacio. Si ese es el caso entonces no hubo errores.
            if (string.IsNullOrEmpty(Mensaje))
            {
                return Objeto_CapaDato.Editar(objeto, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {

            return Objeto_CapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {

            return Objeto_CapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = Objeto_CapaDato.ReestablecerClave(idusuario, CN_Recursos.ConvertiSha256(nuevaclave), out Mensaje);

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
