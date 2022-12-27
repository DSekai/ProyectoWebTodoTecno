using CapaEntidad;
using CapaNegocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestAdmin
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            bool respuesta = true;

            Usuario oUsuario = new Usuario();

            string correo = "izxj_pboaw4@TodoTecno.cl";
            string clave = "123";

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertiSha256(clave)).FirstOrDefault();

            if(oUsuario == null)
            {
                 respuesta = false;
            }
            Assert.AreEqual(false, respuesta );
        }
    }
}
