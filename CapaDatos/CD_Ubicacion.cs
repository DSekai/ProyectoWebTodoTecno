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
    public class CD_Ubicacion
    {
        public List<Region> ObtenerRegion()
        {
            List<Region> lista = new List<Region>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "select * from region";

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
                                new Region()
                                {
                                    //El valor que obtenga de la columna IdUsuaario se convertira a tipo int y se almacenara en la propiedad IdUsuario 
                                    //que le pertenece a la clase Usuario. Lo mismo con las demas columnas.
                                    IdRegion = Convert.ToInt32( dr["IdRegion"]),
                                    Descripcion = dr["Descripcion"].ToString()                                  
                                }
                                );
                        }
                    }
                }
            }
            catch
            {  
                lista = new List<Region>();
            }
            return lista;
        }

        public List<Comuna> ObtenerComuna(int idregion)
        {
            List<Comuna> lista = new List<Comuna>();

            try
            {
                using (MySqlConnection Objeto_conexion = new MySqlConnection(Conexion.cn))
                {

                    string query = "select * from comuna where IdRegion = @IdRegion";

                    MySqlCommand cmd = new MySqlCommand(query, Objeto_conexion);

                    cmd.Parameters.AddWithValue("@IdRegion", idregion);
                    cmd.CommandType = CommandType.Text;

                    Objeto_conexion.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Comuna()
                                {
                                    IdComuna = Convert.ToInt32(dr["IdComuna"]),
                                    Descripcion = dr["Descripcion"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Comuna>();
            }
            return lista;
        }
    }
}
