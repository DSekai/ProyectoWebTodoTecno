using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca Objeto_CapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            //Ocupo metodo Listar de CD_Usuarios.
            return Objeto_CapaDato.Listar();
        }

        public int Registrar(Marca objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion de la Marca no puede quedar vacia";
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
        public bool Editar(Marca objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion de la Marca no puede quedar vacia";
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

        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            return Objeto_CapaDato.ListarMarcaporCategoria(idcategoria);
        }
    }
}
