using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using BundleTransformer.Core.Transformers;

namespace CMNirdesh
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/sidebar.js", "~/Content/botstrap-5.2/js/bootstrap.js",
                      "~/Content/botstrap-5.2/js/bootstrap.min.js", "~/Content/botstrap-5.2/js/bootstrap.bundle.js",
                      "~/Content/botstrap-5.2/js/bootstrap.bundle.min.js",
                      "~/Content/botstrap-5.2/js/bootstrap.esm.js", "~/Content/botstrap-5.2/js/bootstrap.esm.min.js", "~/Content/botstrap-5.2/js/bootstrap.esm.min.js",
                      "~/Content/ow_iteml.carousel.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            //  "~/Scripts/datatables/jquery.dataTables.js",
            //  "~/Scripts/datatables/jszip.min.js",
            //  "~/Scripts/datatables/pdfmake.min.js",
            //  "~/Scripts/datatables/vfs-fonts.js",
            //  "~/Scripts/datatables/buttons.print.min.js",
            //  "~/Scripts/datatables/buttons.html5.min.js"
            //  ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css", "~/Content/style_ms.css", "~/Content/responsive_ms.css", "~/Content/font-awesome_ms.min.css",
                      "~/Content/font-family/stylesheet.css", "~/Content/site.css", "~/Content/styleheader.css", "~/Content/mla-econnect.css",
                      "~/Content/bootstrap-5.2/css/bootstrap.css",
                      "~/Content/bootstrap-5.2/css/bootstrap.min.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-grid.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-grid.min.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-reboot.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-reboot.min.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-utilities.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-utilities.min.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-utilities.rtl.css",
                      "~/Content/bootstrap-5.2/css/bootstrap-utilities.rtl.min.css"
                      ));

            var nullBuilder = new NullBuilder();
            //StyleTransformer and ScriptTransformer classes produce processing of stylesheets and scripts.
            var styleTransformer = new StyleTransformer();

            var scriptTransformer = new ScriptTransformer();
            //NullOrderer class disables the built-in sorting mechanism and save assets sorted in the order they are declared.
            var nullOrderer = new NullOrderer();

            var scriptbundleToObfuscateLogin = new Bundle("~/bundles/LoginJs");
            scriptbundleToObfuscateLogin.Include(
                    "~/Scripts/CryptoJSv3.1.2/rollups/pbkdf2.js",                              
                    "~/Scripts/CryptoJSv3.1.2/components/enc-base64-min.js", "~/Scripts/CryptoJSv3.1.2/components/enc-base64.js",
                    "~/Scripts/Security.js", "~/Scripts/jquery-3.6.4.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/Login.js",
                    "~/Scripts/sidebar.js",
                    "~/Scripts/popper.min.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/bootstrap.bundle.js",
                    "~/Scripts/bootstrap.bundle.min.js",
                    "~/Scripts/bootstrap.esm.js", 
                    "~/Scripts/bootstrap.esm.min.js",
                    "~/Scripts/bootstrap.esm.min.js",
                    "~/Scripts/ow_iteml.carousel.min.js"
                  );
            scriptbundleToObfuscateLogin.Builder = nullBuilder;
            scriptbundleToObfuscateLogin.Transforms.Add(scriptTransformer);
            scriptbundleToObfuscateLogin.Orderer = nullOrderer;
            bundles.Add(scriptbundleToObfuscateLogin);
        }
    }
}
