using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria Objeto_CapaDato = new CD_Categoria();

        public List<Categoria> Listar()
        {
            //Ocupo metodo Listar de CD_Usuarios.
            return Objeto_CapaDato.Listar();
        }

        public int Registrar(Categoria objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion de la Categoria no puede quedar vacia";
            }


            //Aqui validamos si el mensaje sigue vacio. Si ese es el caso entonces no hubo errores.
            if (string.IsNullOrEmpty(Mensaje))
            {


                return Objeto_CapaDato.Registrar(objeto, out Mensaje);
            }
            else
            {
                //Retornamos el valor de 0 para saber que no se ha creado un usuario.
                return 0;
            }


        }
        public bool Editar(Categoria objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion de la Categoria no puede quedar vacia";
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
    }
}
