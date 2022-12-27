using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Producto Objeto_Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string IdTransaccion { get; set; } /*Este campo no esta en la tabla detalleventa pero lo integro para saber el numero de transaccion para metodo paypal*/
        public List<DetalleVenta> Objeto_DetalleVenta { get; set; }
    }
}
