using System.Web;
using System.Web.Optimization;

namespace CapaPresentacionAdmin
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/complementos").Include(
            "~/Scripts/scripts.js",
            //Importe jquery.datatable y designe los js.
            "~/Scripts/DataTables/jquery.dataTables.js",
            "~/Scripts/DataTables/dataTables.responsive.js",
            //Manu importo la animacion de carga
            "~/Scripts/loadingoverlay/loadingoverlay.min.js",
            //Manu importo la alerta de bootstrap
            "~/Scripts/sweetalert.min.js",
            // Validaciones de datos
            "~/Scripts/jquery.validate.js",
            // Importar calendario
            "~/Scripts/jquery-ui.js",
            //Importe fontawesome y designe su js.
            "~/Scripts/fontawesome/all.min.js"));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css",
                      //Importe jquery.datatable y designe los css.
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/responsive.dataTables.css",
                      //Manu importa tambien el archivo del sweetalert de la carpeta content
                      "~/Content/sweetalert.css",
                      "~/Content/jquery-ui.css"
                      ));
        }
    }
}
