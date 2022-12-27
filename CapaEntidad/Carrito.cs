using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente Objeto_Cliente { get; set; }
        public Producto Objeto_Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
