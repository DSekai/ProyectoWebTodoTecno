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
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn)) 
                {

                    string query = "select IdUsuario,Nombres,Apellidos,Correo,Clave,Reestablecer,Activo from usuario";

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
                                new Usuario() 
                                {
                                    //El valor que obtenga de la columna IdUsuaario se convertira a tipo int y se almacenara en la propiedad IdUsuario 
                                    //que le pertenece a la clase Usuario. Lo mismo con las demas columnas.
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch {
                lista = new List<Usuario>();
            }
            return lista;
        }

        public int Registrar(Usuario objeto, out string Mensaje)
        {
            //Aqui se almacenara el id del usuario que se ha agregado.
            int idautogenerado = 0;
            //Dejamos valor de mensaje como vacio.
            Mensaje = string.Empty;

            try {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn)) 
                {

                    string query = "sp_usuario_insertar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_nombres", objeto.Nombres);
                    cmd.Parameters.AddWithValue("_apellidos", objeto.Apellidos);
                    cmd.Parameters.AddWithValue("_correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("_clave", objeto.Clave);
                    cmd.Parameters.AddWithValue("_activo", objeto.Activo);

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
            catch (Exception ex) {

                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }

        public bool Editar(Usuario objeto, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "sp_usuario_editar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    //Le pasamos los valores almacenados de la clase usuario (Usuario objeto) a los parametros del procedure,
                    cmd.Parameters.AddWithValue("_id", objeto.IdUsuario);
                    cmd.Parameters.AddWithValue("_nombres", objeto.Nombres);
                    cmd.Parameters.AddWithValue("_apellidos", objeto.Apellidos);
                    cmd.Parameters.AddWithValue("_correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("_clave", objeto.Clave);
                    cmd.Parameters.AddWithValue("_activo", objeto.Activo);
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

                    string query = "sp_usuario_eliminar";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);
                    cmd.Parameters.AddWithValue("_id", id);
                    //cmd.Parameters.Add("Resultado", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    Objeto_conexion.Open();
                    //Si sale todo bien, el resultado es true ya que si afecto 1 regristro, de lo contrario es false si falla y no afecto nada (0).
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    //Mensaje = "todo ok";
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {

            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    MySqlCommand cmd = new MySqlCommand("update usuario set clave = @nuevaclave, reestablecer = 0 where idusuario = @id", Objeto_conexion);
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

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {

            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {
                    MySqlCommand cmd = new MySqlCommand("update usuario set clave = @clave, reestablecer = 1 where idusuario = @id", Objeto_conexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
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
