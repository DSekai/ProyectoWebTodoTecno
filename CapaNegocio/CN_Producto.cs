using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto Objeto_CapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            //Ocupo metodo Listar de CD_Usuarios.
            return Objeto_CapaDato.Listar();
        }

        public int Registrar(Producto objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            //Condiciones para saber si el valor es nullo o es vacio.
            if (string.IsNullOrEmpty(objeto.Nombre) || string.IsNullOrWhiteSpace(objeto.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede ser vacio";
            }

            else if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion del Producto no puede ser vacio";
            }

            else if (objeto.Objeto_Marca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca";
            }

            else if (objeto.Objeto_Categoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }

            else if (objeto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del Producto";
            }

            else if (objeto.Stock == 0)
            {
                Mensaje = "Debe ingresar stock dekl Producto";
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
        public bool Editar(Producto objeto, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Nombre) || string.IsNullOrWhiteSpace(objeto.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede ser vacio";
            }

            else if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "La Descripcion del Producto no puede ser vacio";
            }

            else if (objeto.Objeto_Marca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca";
            }

            else if (objeto.Objeto_Categoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }

            else if (objeto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del Producto";
            }

            else if (objeto.Stock == 0)
            {
                Mensaje = "Debe ingresar stock dekl Producto";
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

        public bool GuardarDatosImagen(Producto objeto_Producto, out string Mensaje)
        {
            return Objeto_CapaDato.GuardarDatosImagen(objeto_Producto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {

            return Objeto_CapaDato.Eliminar(id, out Mensaje);
        }
    }
}

