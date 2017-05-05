using System.Web;
using System.Web.Optimization;

namespace SISSDV1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.contextMenu.js",
                        "~/Scripts/jquery.ui.position.js"));            

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/ripples.js",
                      "~/Scripts/material.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapmd").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/jquery.scrollTo.min.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/jquery.app.js"));

            bundles.Add(new ScriptBundle("~/bundles/funcionalidades").Include(
                      "~/Scripts/jquery-3.1.0.min.js",
                      "~/Scripts/jquery2.0.2.js",
                      "~/Scripts/jquery-1.10.2.js",
                      "~/Scripts/jquery.contextMenu.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/modal.js",
                      "~/Scripts/filtrotabelas.js",
                      "~/Scripts/funcoes.js"));

            bundles.Add(new ScriptBundle("~/bundles/modal").Include(
                        "~/Scripts/modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-reset.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/animate.css",
                      "~/Content/style.css",
                      "~/Content/ionicons.min.css",                      
                      "~/Content/Site.css",
                      "~/Content/full-calendar/fullcalendar.css",
                      "~/Content/material-dashboard.css"));

            bundles.Add(new StyleBundle("~/Content/css1").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-material-design.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/Content/estilo.css"));
        }
    }
}
