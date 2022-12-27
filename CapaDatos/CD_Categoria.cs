using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using MySql.Data.MySqlClient;

namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "select IdCategoria, Descripcion, Activo from categoria";

                    //Comando para ejecutar la consulta
                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.CommandType = CommandType.Text;

                    Objeto_conexion.Open();

                    //MySqlDataReader nos permite dar lectura al resultado de nuestro comando
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Categoria>();
            }
            return lista;
        }

        public int Registrar(Categoria objeto, out string Mensaje)
        {
            //Aqui se almacenara el id del usuario que se ha agregado.
            int idautogenerado = 0;
            //Dejamos valor de mensaje como vacio.
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_categoria_registrar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("_Descripcion", objeto.Descripcion);
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

        public bool Editar(Categoria objeto, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_categoria_editar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_Id", objeto.IdCategoria);
                    cmd.Parameters.AddWithValue("_Descripcion", objeto.Descripcion);
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

                    string query = "sp_categoria_eliminar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_Id", id);

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

    }
}
