using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Importar en Agregar/Referencia
using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        //Se obtiene valor de la cadena de conexion y se guarda en variable cn
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
