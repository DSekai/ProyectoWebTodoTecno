using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca Objeto_Marca { get; set; }
        public Categoria Objeto_Categoria { get; set; }
        public decimal Precio { get; set; }

        // Ocupado para validar la conversion a un formato decimal de la region.
        public string PrecioTexto { get; set; }
        public int Stock { get; set; }
        public string RutaImagen { get; set; }
        public string NombreImagen { get; set; }
        public bool Activo { get; set; }

        // Para almacenar el formato de la imagen (Base64).
        public string Base64 { get; set; }
        // Tipo de imagen (jpeg, png etc..).
        public string Extension { get; set; }
    }
}
