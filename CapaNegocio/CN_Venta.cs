using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta Objeto_CapaDato = new CD_Venta();

        public int RegistrarVenta(Venta objeto, out string Mensaje)
        {
            return Objeto_CapaDato.RegistrarVenta(objeto, out Mensaje);
        }

        public bool Registrar(int idventa, int idproducto, int cantidad, int total, out string Mensaje)
        {
            return Objeto_CapaDato.RegistrarDetalleVenta(idventa, idproducto, cantidad, total, out Mensaje);
        }

        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            return Objeto_CapaDato.ListarCompras(idcliente);
        }
    }
}
