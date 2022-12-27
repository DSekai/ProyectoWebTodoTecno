using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace CapaPresentacionTienda.Filter
{
    // Con esta herencia ActionFilterAttribute, se puede ocupar un metodo que se ejecuta
    // cuando una vista se muestra
    public class ValidarSessionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Si el cliente es igual a nulo, se va a redireccionar al login
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}