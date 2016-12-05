using System.Web;
using System.Web.Optimization;

namespace ACLager {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/material.js",
                      "~/Scripts/ripples.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/bootstrap-material-datetimepicker.js",
                      "~/Scripts/list.min.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/jquery.transit.js",
                      "~/Scripts/snackbar.min.js",
                      "~/Scripts/select2.full.js",
                      "~/Scripts/i18n/da.js",
                      "~/Scripts/cldr.js",
                      "~/Scripts/event.js",
                      "~/Scripts/supplemental.js",
                      "~/Scripts/globalize.js",
                      "~/Scripts/number.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ripples.css",
                      "~/Content/select2.css"
                      //"~/Content/bootstrap-material-datetimepicker.css"
                      ));
        }
    }
}
