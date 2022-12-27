using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        // ++++++++++++++++++++++ CATEGORIA ++++++++++++++++++++++

        #region CATEGORIA
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            //Este object nos permite almacenar cualquier tipo de resultado, string, int etc..
            object Resultado;
            string Mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                //El metodo nos devolvera el id del usuario y lo almacenara el Resultado.
                Resultado = new CN_Categoria().Registrar(objeto, out Mensaje);
            }
            else
            {
                Resultado = new CN_Categoria().Editar(objeto, out Mensaje);
            }

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EleminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        // ++++++++++++++++++++++ MARCA ++++++++++++++++++++++

        #region MARCA

        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();

            oLista = new CN_Marca().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            //Este object nos permite almacenar cualquier tipo de resultado, string, int etc..
            object Resultado;
            string Mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                //El metodo nos devolvera el id del usuario y lo almacenara el Resultado.
                Resultado = new CN_Marca().Registrar(objeto, out Mensaje);
            }
            else
            {
                Resultado = new CN_Marca().Editar(objeto, out Mensaje);
            }

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EleminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        // ++++++++++++++++++++++ PRODUCTO ++++++++++++++++++++++

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();

            oLista = new CN_Producto().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // Se cambia el tipo de dato de la variable objeto a string ya que se va a trabajar con
        // distintos elementos y con imagenes.
        // Variable tipo HttpPostedFileBase para recibir la imagen del producto.
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string Mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            // Validacion y conversion de PrecioTexto a decimal reginal.
            // NumberStyles.AllowDecimalPoint para que se considere los decimales como puntos.
            // new CultureInfo("es-CL") para que considere la cultura reginal de Chile.
            // Por ultimo, el resultado de la conversion se guardara en  out precio.
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-CL"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, Mensaje = "El Formato del Precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out Mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out Mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    // Ruta obtenida de Web.Config
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    // Obtencion de extension del archivo
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    // string.Concat para concatenar el id del producto + la extension
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out Mensaje);
                    }
                    else
                    {
                        Mensaje = "Se guardo el Producto pero hubo problemas con la Imagen";
                    }
                }
            }

            return Json(new { operacion_exitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, Mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }



        #endregion
    }
}