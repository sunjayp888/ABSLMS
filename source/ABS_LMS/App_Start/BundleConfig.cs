using System.Web;
using System.Web.Optimization;

namespace ABS_LMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
         bundles.Add(new ScriptBundle("~/bundles/jquerylib").Include("~/Scripts/jquery-1.11.1.min.js",
                                                           "~/Scripts/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.validate.min.js"
            //  ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.unobtrusive*",
                            "~/Scripts/jquery.validate*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/themes/LMS/css/LMSMain.css"));
            bundles.Add(new StyleBundle("~/bundles/base").Include("~/Content/base/jquery.ui.core.css",
                                                                "~/Content/base/jquery.ui.datepicker.css",
                                                                "~/Content/base/jquery.ui.theme.css",
                                                                "~/Content/base/jquery.ui.all.css"
                                                                ));
        }
    }
}
