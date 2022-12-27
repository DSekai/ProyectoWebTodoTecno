using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion Objeto_CapaDato = new CD_Ubicacion();

        public List<Region> ObtenerRegion()
        {
            return Objeto_CapaDato.ObtenerRegion();
        }
        public List<Comuna> ObtenerComuna(int idregion)
        {
            return Objeto_CapaDato.ObtenerComuna(idregion);
        }


    }
}
