using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_ExisteCarrito";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("_IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("_IdProducto", idproducto);

                    //Parametros de salida.
                    cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    cmd.ExecuteNonQuery();

                    //Retorno de los parametros de salida de Resultado y Mensaje.
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {

                resultado = false;
            }
            return resultado;
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn);
            Objeto_conexion.Open();

            MySqlTransaction tr;
            tr = Objeto_conexion.BeginTransaction();

            try
            {
                string query = "sp_OperacionCarrito";

                MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion, tr);

                cmd.Parameters.AddWithValue("_IdCliente", idcliente);
                cmd.Parameters.AddWithValue("_IdProducto", idproducto);
                cmd.Parameters.AddWithValue("_sumar", sumar);

                cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                tr.Commit();

                resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;


            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    MySqlCommand cmd = new MySqlCommand("select count(*) from carrito where IdCliente = @IdCliente", Objeto_conexion);
                    cmd.Parameters.AddWithValue("@IdCliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    Objeto_conexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_ObtenerCarritoCliente";

                    //Comando para ejecutar la consulta
                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("_IdCliente", idcliente);

                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    //MySqlDataReader nos permite dar lectura al resultado de nuestro comando
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                Objeto_Producto = new Producto
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Objeto_Marca = new Marca() { Descripcion = dr["DesMarca"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CL")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])


                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<Carrito>();
            }
            return lista;
        }
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn);
            Objeto_conexion.Open();

            MySqlTransaction tr;
            tr = Objeto_conexion.BeginTransaction();

            try
            {
                string query = "sp_EliminarCarrito";

                MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion, tr);

                cmd.Parameters.AddWithValue("_IdCliente", idcliente);
                cmd.Parameters.AddWithValue("_IdProducto", idproducto);

                //Parametros de salida.
                cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                //Objeto_conexion.Open();

                cmd.ExecuteNonQuery();

                tr.Commit();

                //Retorno de los parametros de salida de Resultado y Mensaje.
                resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

            }
            catch (Exception ex)
            {
                tr.Rollback();
                resultado = false;
            }
            return resultado;
        }
    }
}
