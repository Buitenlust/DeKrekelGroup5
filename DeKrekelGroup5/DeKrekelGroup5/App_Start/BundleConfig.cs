using System.Web;
using System.Web.Optimization;

namespace DeKrekelGroup5
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/home").Include(
            //    "~/Scripts/jquery.min.js",
            //    "~/Scripts/respond.min.js",
            //    "~/Scripts/jquery.reveal.js",
            //     "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/maincss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/masterLayout.css"));

            bundles.Add(new StyleBundle("~/Content/allcss").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/site.css",
                     "~/Content/masterLayout.css",
                     "~/Content/uploadfile.css" ));

            //bundles.Add(new ScriptBundle("~/bundles/main").Include(
            //    "~/Scripts/jquery.min.js",
            //    "~/Scripts/jQuery.MultiFile.min.js",
            //    "~/Scripts/jquery.validate*",
            //    "~/Scripts/respond.min.js",
            //    "~/Scripts/jquery.reveal.js",
            //     "~/Scripts/jquery.unobtrusive-ajax.min.js"));


            //bundles.Add(new ScriptBundle("~/bundles/all").Include(
            //    "~/Scripts/jquery.min.js",
            //    "~/Scripts/respond.min.js",
            //    "~/Scripts/jquery.reveal.js",
            //    "~/Scripts/jQuery.MultiFile.min.js",
            //    "~/Scripts/upload.js",
            //    "~/Scripts/jquery.validate.min.js",
            //    "~/Scripts/jquery.validate.unobtrusive.min.js",
            //    "~/Scripts/jquery.unobtrusive-ajax.min.js"));
        }
    }
}
