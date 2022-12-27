using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;

using CapaEntidad.Paypal;
//using System.Net;
//using NodaMoney;

using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        List<Carrito> olista;

        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }

            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategoria()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();

            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Objeto_Marca = p.Objeto_Marca,
                Objeto_Categoria = p.Objeto_Categoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
            p.Objeto_Categoria.IdCategoria == (idcategoria == 0 ? p.Objeto_Categoria.IdCategoria : idcategoria) &&
            p.Objeto_Marca.IdMarca == (idmarca == 0 ? p.Objeto_Marca.IdMarca : idmarca) &&
            p.Stock > 0 && p.Activo == true).ToList();

            var Jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            Jsonresult.MaxJsonLength = int.MaxValue;

            return Jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {

            // Conversion a objeto cliente para obtener solo su id
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarProductoCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new Carrito()
            {
                Objeto_Producto = new Producto()
                {
                    IdProducto = oc.Objeto_Producto.IdProducto,
                    Nombre = oc.Objeto_Producto.Nombre,
                    Objeto_Marca = oc.Objeto_Producto.Objeto_Marca,
                    Precio = oc.Objeto_Producto.Precio,
                    RutaImagen = oc.Objeto_Producto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.Objeto_Producto.RutaImagen, oc.Objeto_Producto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.Objeto_Producto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {

            // Conversion a objeto cliente para obtener solo su id
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerRegion()
        {
            List<Region> oLista = new List<Region>();

            oLista = new CN_Ubicacion().ObtenerRegion();

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerComuna(int idregion)
        {
            List<Comuna> oLista = new List<Comuna>();

            oLista = new CN_Ubicacion().ObtenerComuna(idregion);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        { 
            decimal total = 0;
            decimal total2 = 0;
            string a;
            string b;
            //Convert.ToDecimal(total, new CultureInfo("es-CL"));

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-CL");
            detalle_venta.Columns.Add("IdProducto", typeof(int));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();

            TempData["testlista"] = oListaCarrito;

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal pproducto = oCarrito.Objeto_Producto.Precio;
                decimal pdolar = Convert.ToDecimal(oVenta.Dolar.ToString().Replace(".",","));
                int dolarXproducto = Convert.ToInt32(pproducto / pdolar);
                double dolarXproducto2 = Convert.ToDouble(pproducto / pdolar);
                string test = dolarXproducto2.ToString("N2", new CultureInfo("es-CL")).Replace(",", ".");
                b = test;

                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.Objeto_Producto.Precio;
                decimal subtotal2 = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * Convert.ToDecimal(dolarXproducto2.ToString("N2"));

                total += subtotal;
                total2 += subtotal2;

                 a = total2.ToString("G", new CultureInfo("es-CL")).Replace(".", ",");


                oListaItem.Add(new Item()
                {
                    name = oCarrito.Objeto_Producto.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = b
                    }


                });

                detalle_venta.Rows.Add(new object[] {

                    oCarrito.Objeto_Producto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });

                //var testid = oCarrito.Objeto_Producto.IdProducto;
                //var testcant = oCarrito.Cantidad;
                ////var testprecio = oCarrito.Objeto_Producto.Precio;
                //bool respuesta = false;
                //string mensaje = string.Empty;

                //respuesta = new CN_Venta().Registrar(testid, testcant, Convert.ToInt32(subtotal), out mensaje);

            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total2.ToString("G", new CultureInfo("es-CL")).Replace(",", "."),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total2.ToString("G", new CultureInfo("es-CL")).Replace(",", ".")
                        }
                    }
                },
                description = "Compra de articulo de mi tienda",
                items = oListaItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() {purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "TodoTecno.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44394/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44394/Tienda/Carrito"
                }
            };

            //Obtener_dolar_Load();

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            response_paypal = await opaypal.CrearSolicitud(oCheckOutOrder);

            return Json(response_paypal, JsonRequestBehavior.AllowGet );

        }

        [HttpPost]
        public JsonResult RegistroDetalleVenta(int idventa, int idproducto, int cantidad, int total)
        {

            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Venta().Registrar(idventa, idproducto, cantidad, total, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {

            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();
            response_Paypal = await opaypal.AprobarPago(token);


            ViewData["Status"] = response_Paypal.Status;

            if (response_Paypal.Status)
            {
                olista = (List<Carrito>)TempData["testlista"];

                Venta oVenta = (Venta)TempData["Venta"];

                oVenta.IdTransaccion = response_Paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;

                int respuesta = new CN_Venta().RegistrarVenta(oVenta, out mensaje);
                int idventa = respuesta;

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;

                foreach (Carrito item in olista)
                {
                    int idproducto = item.Objeto_Producto.IdProducto;
                    int cantidad = item.Cantidad;
                    decimal precio = item.Objeto_Producto.Precio;
                    decimal subtotal = cantidad * precio;

                    bool respuesta2 = new CN_Venta().Registrar(idventa, idproducto, cantidad, Convert.ToInt32(subtotal), out mensaje);
                }

            }

            return View();
        }

        //private string Obtener_dolar_Load()
        //{
        //    //Obtener cadena de texto desde una web
        //    //var url = "https://www.xe.com/es/currencyconverter/convert/?Amount=1&From=CLP&To=USD";

        //    //var textFromFile = (new WebClient()).DownloadString(url);

        //    ////Seleccionar los caracteres correspondientes al dolar

        //    //string clp;

        //    //char dolar1 = textFromFile[139533];
        //    //char dolar2 = textFromFile[139534];
        //    //char dolar3 = textFromFile[139535];
        //    //char dolar4 = textFromFile[139536];
        //    //char dolar5 = textFromFile[1395337];
        //    //char dolar6 = textFromFile[139538];
        //    ////char dolar7 = textFromFile[972668];
        //    //clp = dolar1.ToString() + dolar2.ToString() + dolar3.ToString() + dolar4.ToString() + dolar5.ToString() + dolar6.ToString() /*+ dolar7.ToString()*/;


        //    var dolara = Money.USDollar(1);

        //    string a = "";

        //    return a;

        //}

        
        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<DetalleVenta> oLista = new List<DetalleVenta>();

            bool conversion;

            oLista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
            {
                Objeto_Producto = new Producto()
                {
                    Nombre = oc.Objeto_Producto.Nombre,
                    Precio = oc.Objeto_Producto.Precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.Objeto_Producto.RutaImagen, oc.Objeto_Producto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.Objeto_Producto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(oLista);
        }
    }
}