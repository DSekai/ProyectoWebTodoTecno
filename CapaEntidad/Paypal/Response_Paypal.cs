using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Paypal
{
    // <T> significa que se enviara una clase generica
    // y Se va a devolver mediante public T Response
    public class Response_Paypal<T>
    {
        public bool Status { get; set; }
        public T Response { get; set; }
    }
}
