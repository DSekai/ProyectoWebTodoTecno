﻿using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Invitado()
        {
            Cliente oCliente = null;

            string correo = "@Invitado";


            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

            Session["Cliente"] = oCliente;
            Session["Invitado"] = true;

            ViewBag.Error = null;
            return RedirectToAction("Index", "Tienda");



        }

        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if(objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertiSha256(clave)).FirstOrDefault();

            if(oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

                    Session["Cliente"] = oCliente;
                    Session["Invitado"] = null;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente cliente = new Cliente();

            cliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string IdCliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(IdCliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertiSha256(claveactual))
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vclave"] = "";

                ViewBag.Error = "La Contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = IdCliente;
                // Variable usada para mantener el valor de la contraseña cuando sea la correcta (se enlaza con el input claveactual).
                ViewData["vclave"] = claveactual;

                ViewBag.Error = "Las Contraseñas no son iguales";
                return View();

            }

            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConvertiSha256(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(IdCliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = IdCliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }
        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }

        //[HttpPost]
        //public ActionResult Invitado(bool ok)
        //{
        //    ok = true;

        //    Cliente oCliente = null;

        //    string correo = "@Invitado";
           

        //    oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

        //    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

        //    Session["Cliente"] = oCliente;
        //    Session["Invitado"] = true;

        //    ViewBag.Error = null;
        //    return RedirectToAction("Index", "Tienda");



        //}

    }
}