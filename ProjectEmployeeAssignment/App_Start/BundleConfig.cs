using System.Web;
using System.Web.Optimization;

namespace ProjectEmployeeAssignment
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/header").Include(
                        "~/Content/bootstrap.min.css",
                         "~/Content/font-awesome.min.css",
                         "~/Content/fontawesome.min.css",
                         "~/Content/fontawesome-all.min.css",
                         "~/Content/v4-shims.css",
                         "~/Content/css/pea-navbar-stylesheet.css",
                         "~/Content/css/pea-menu-stylesheet.css",
                         "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                         "~/Content/DataTables/css/dataTables.bootstrap4.min.css",
                         "~/Content/DataTables/css/responsive.dataTables.css",
                         "~/Content/css/gijgo.min.css",
                         "~/Content/css/jquery.easing.min.js",
                         "~/Content/css/select2.css",
                         "~/Content/css/editor.css",
                         "~/Content/css/pea.css",
                         "~/Content/css/select2-bootstrap4.css"));

            bundles.Add(new ScriptBundle("~/scripts/header").Include(
                        "~/Scripts/jquery-3.4.1.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/editor.js"
                       ));

            bundles.Add(new ScriptBundle("~/scripts/footer").Include(
                       "~/Scripts/bootstrap.bundle.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js",
                        "~/Scripts/DataTables/dataTables.responsive.min.js",
                        "~/Scripts/js/gijgo.min.js",
                        "~/Scripts/js/ims-common.js",
                         "~/Scripts/js/select2.js",

                        "~/Scripts/js/jquery.tabletojson.js"));
            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                       "~/Scripts/js/common.js"));
            bundles.Add(new ScriptBundle("~/scripts/login").Include(
                       "~/Scripts/js/login.js"));
            bundles.Add(new ScriptBundle("~/scripts/dashboard").Include(
                       "~/Scripts/js/dashboard.js"));
        }
    }
}
