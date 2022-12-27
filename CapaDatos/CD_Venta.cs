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
    public class CD_Venta
    {
        public int RegistrarVenta(Venta objeto, out string Mensaje)
        {
            int Respuesta = 0;
            Mensaje = string.Empty;

            MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn);
            Objeto_conexion.Open();

            MySqlTransaction tr;
            tr = Objeto_conexion.BeginTransaction();

            try
            {

                string query = "sp_Registrar_Venta";

                MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion, tr);

                cmd.Parameters.AddWithValue("_IdCliente", objeto.IdCliente);
                cmd.Parameters.AddWithValue("_TotalProducto", objeto.TotalProducto);
                cmd.Parameters.AddWithValue("_MontoTotal", objeto.MontoTotal);
                cmd.Parameters.AddWithValue("_Contacto", objeto.Contacto);
                cmd.Parameters.AddWithValue("_IdRegion", objeto.IdRegion);
                cmd.Parameters.AddWithValue("_Telefono", objeto.Telefono);
                cmd.Parameters.AddWithValue("_Direccion", objeto.Direccion);
                cmd.Parameters.AddWithValue("_IdTransaccion", objeto.IdTransaccion);

                cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                tr.Commit();

                Respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            }
            catch (Exception ex)
            {
                tr.Rollback();
                Respuesta = 0;
                Mensaje = ex.Message;
            }
            return Respuesta;
        }

        public bool RegistrarDetalleVenta(int idventa, int idproducto, int cantidad, int total, out string Mensaje)
        {

            bool Respuesta = false;
            Mensaje = string.Empty;

            MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn);
            Objeto_conexion.Open();

            MySqlTransaction tr;
            tr = Objeto_conexion.BeginTransaction();
            try
            {

                string query = "sp_Registrar_DetalleVenta";

                MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion, tr);

                cmd.Parameters.AddWithValue("_IdVenta", idventa);
                cmd.Parameters.AddWithValue("_IdProducto", idproducto);
                cmd.Parameters.AddWithValue("_Cantidad", cantidad);
                cmd.Parameters.AddWithValue("_TotalProducto", total);


                cmd.Parameters.Add("Resultado", MySqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", MySqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                tr.Commit();


                Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            catch (Exception ex)
            {
                tr.Rollback();
                Respuesta = false;
                Mensaje = ex.Message;
            }
            return Respuesta;
        }

        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_ListarCompra";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("_IdCliente", idcliente);

                    cmd.CommandType = CommandType.StoredProcedure;

                    Objeto_conexion.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {
                                Objeto_Producto = new Producto
                                {
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CL")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },

                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-CL")),
                                IdTransaccion = dr["IdTransaccion"].ToString()


                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<DetalleVenta>();
            }
            return lista;
        }
    }
}

