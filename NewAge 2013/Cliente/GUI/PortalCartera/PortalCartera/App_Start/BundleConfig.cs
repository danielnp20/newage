using System.Web.Optimization;

namespace NewAge.Cliente.GUI.PortalCartera
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/underscore").Include("~/Scripts/Underscore/underscore-min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/placeholder").Include("~/Scripts/jquery.placeholder.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/kendoui").Include("~/Scripts/kendo/2014.1.318/kendo.web.min.js", "~/Scripts/kendo/2014.1.318/cultures/kendo.culture.es-CO.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
           
            //CSS.css
            bundles.Add(new StyleBundle("~/Content/bootstrap.css").Include("~/Content/bootstrap/css/bootstrap.css", "~/Content/bootstrap/css/bootstrap-theme.css"));
            bundles.Add(new StyleBundle("~/Content/kendoui.css").Include("~/Content/kendo/2014.1.318/kendo.common.min.css", "~/Content/kendo/2014.1.318/kendo.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/css.css").Include("~/Content/site.css", "~/Content/ajax.update.progress.css"));

            //Mentor libraries
            bundles.Add(new ScriptBundle("~/bundles/newage.base").Include("~/Scripts/NewAge/ajax.update.progress.js", "~/Scripts/NewAge/base.core.js"));
            bundles.Add(new ScriptBundle("~/bundles/newage.general").Include("~/Scripts/NewAge/general.js"));
            bundles.Add(new ScriptBundle("~/bundles/newage.login").Include("~/Scripts/NewAge/login.js"));
            bundles.Add(new ScriptBundle("~/bundles/newage.searchcredits").Include("~/Scripts/NewAge/searchCredits.js"));
        }
    }
}
