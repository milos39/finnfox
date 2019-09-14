using System.Web;
using System.Web.Optimization;

namespace finnfox
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bs-datepicker").Include(
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datepicker.rs-latin.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bs-table").Include(
                      "~/Scripts/bootstrap-table.js",
                      "~/Scripts/bootstrap-table-hr-HR.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-racPromene").Include(
                      "~/Scripts/NGracunovodstvenePromene.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-globalnaAnalitika").Include(
                      "~/Scripts/NGglobalnaAnalitika.js"));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                      "~/Scripts/Chart.bundle.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Scripts/select2/select2.js",
                      "~/Scripts/select2/sr.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/bootstrap-lumen.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-table.css",
                      "~/Content/Chart.css",
                      "~/Content/select2.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/switch").Include(
                      "~/Content/switch.css"));


        }
    }
}
