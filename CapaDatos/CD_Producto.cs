using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_Producto_Selectivo";

                    //Comando para ejecutar la consulta
                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);
                        
                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    //MySqlDataReader nos permite dar lectura al resultado de nuestro comando
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Objeto_Marca = new Marca() {IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString()},
                                Objeto_Categoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString()},
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CL")),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                
                lista = new List<Producto>();
            }
            return lista;
        }

        public int Registrar(Producto objeto, out string Mensaje)
        {
            //Aqui se almacenara el id del usuario que se ha agregado.
            int idautogenerado = 0;
            //Dejamos valor de mensaje como vacio.
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    string query = "sp_producto_ingresar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("_Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("_Descripcion", objeto.Descripcion);
                    cmd.Parameters.AddWithValue("_IdMarca", objeto.Objeto_Marca.IdMarca);
                    cmd.Parameters.AddWithValue("_IdCategoria", objeto.Objeto_Categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("_Precio", objeto.Precio);
                    cmd.Parameters.AddWithValue("_Stock", objeto.Stock);
                    cmd.Parameters.AddWithValue("_Activo", objeto.Activo);

                    cmd.Parameters.Add("Resultado", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Indiacamos el tipo de comando a ejecutar.
                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    cmd.ExecuteNonQuery();

                    //Retorno de los parametros de salida de Resultado y Mensaje.
                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }
        public bool Editar(Producto objeto, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_Producto_editar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_IdProducto", objeto.IdProducto);
                    cmd.Parameters.AddWithValue("_Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("_Descripcion", objeto.Descripcion);
                    cmd.Parameters.AddWithValue("_IdMarca", objeto.Objeto_Marca.IdMarca);
                    cmd.Parameters.AddWithValue("_IdCategoria", objeto.Objeto_Categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("_Precio", objeto.Precio);
                    cmd.Parameters.AddWithValue("_Stock", objeto.Stock);
                    cmd.Parameters.AddWithValue("_Activo", objeto.Activo);

                    //Parametros de salida.
                    cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Indiacamos el tipo de comando a ejecutar.
                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_producto_eliminar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_IdProducto", id);

                    //Parametros de salida.
                    cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Indiacamos el tipo de comando a ejecutar.
                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool GuardarDatosImagen(Producto objeto_Producto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    string query = "sp_producto_Imagen";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);
                    cmd.Parameters.AddWithValue("_RutaImagen", objeto_Producto.RutaImagen);
                    cmd.Parameters.AddWithValue("_NombreImagen", objeto_Producto.NombreImagen);
                    cmd.Parameters.AddWithValue("_IdProducto", objeto_Producto.IdProducto);

                    //cmd.Parameters.Add("Resultado", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Indiacamos el tipo de comando a ejecutar.
                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }

                    //Retorno de los parametros de salida de Resultado y Mensaje.
                    //idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    //Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
