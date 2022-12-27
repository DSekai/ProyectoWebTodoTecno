using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; } /*En el video dice que no colocara referencia a la clase cliente*/
        public int TotalProducto { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoTotalPaypal { get; set; }
        public string Contacto { get; set; }
        public string IdRegion { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string FechaTexto { get; set; }
        public string IdTransaccion { get; set; }
        public string Dolar { get; set; }
    }
}
