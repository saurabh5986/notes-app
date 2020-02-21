using System.Web.Optimization;

namespace NotesApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Template/assets/libs/jquery/dist/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/Template/assets/libs/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/Template/dist/js/app.min.js",
                        "~/Content/Template/dist/js/app.init.js",
                        "~/Content/Template/dist/js/app-style-switcher.js",
                        "~/Content/Template/assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js",
                        "~/Content/Template/assets/extra-libs/sparkline/sparkline.js",
                        "~/Content/Template/dist/js/waves.js",
                        "~/Content/Template/dist/js/sidebarmenu.js",
                        "~/Content/Template/dist/js/custom.min.js",
                        "~/Content/Template/assets/extra-libs/DataTables/datatables.min.js",
                        "~/Content/Template/assets/libs/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Template/dist/css/style.min.css",
                      "~/Content/Template/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.css",
                      "~/Content/Template/assets/libs/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"));
        }
    }
}