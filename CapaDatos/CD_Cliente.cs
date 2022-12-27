using CapaEntidad;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "select IdCliente,Nombres,Apellidos,Correo,Clave,Reestablecer from Cliente";

                    //Comando para ejecutar la consulta
                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.CommandType = CommandType.Text;

                    Objeto_conexion.Open();

                    //MySqlDataReader nos permite dar lectura al resultado de nuestro comando
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Cliente()
                                {
                                    //El valor que obtenga de la columna IdUsuaario se convertira a tipo int y se almacenara en la propiedad IdCliente 
                                    //que le pertenece a la clase Cliente. Lo mismo con las demas columnas.
                                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"])
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Cliente>();
            }
            return lista;
        }
        public int Registrar(Cliente objeto, out string Mensaje)
        {
            //Aqui se almacenara el id del Cliente que se ha agregado.
            int idautogenerado = 0;
            //Dejamos valor de mensaje como vacio.
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_RegistrarCliente";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase Cliente (Cliente objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_Nombres", objeto.Nombres);
                    cmd.Parameters.AddWithValue("_Apellidos", objeto.Apellidos);
                    cmd.Parameters.AddWithValue("_Correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("_Clave", objeto.Clave);

                    //Parametros de salida.
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
        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {

            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    MySqlCommand cmd = new MySqlCommand("update cliente set clave = @nuevaclave, reestablecer = 0 where idcliente = @id", Objeto_conexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    Objeto_conexion.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }
        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {

            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    MySqlCommand cmd = new MySqlCommand("update cliente set clave = @clave, reestablecer = 1 where idcliente = @id", Objeto_conexion);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    Objeto_conexion.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }
    }
}
