using System.Web;
using System.Web.Optimization;

namespace ScoutSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/modernizr").Include(
                       "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/Bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Bundles/signature_pad").Include(
                    "~/Scripts/signature_pad.js"
                ));

            bundles.Add(new Bundle("~/Bundles/ckeditor").Include(
                    "~/Scripts/ckeditor/ckeditor.js"
                ));

            //As Bundle not ScriptBundle (ScriptBundle minifys scripts which will break them)
            bundles.Add(new Bundle("~/Bundles/angular").Include(
                   "~/Scripts/angular.min.1.5.7.js",
                   "~/Scripts/angular-animate.min.1.5.7.js",
                   "~/Scripts/angular-datepicker.js",
                   "~/Scripts/angular-sanitize.min.1.5.7.js"

                ));

            //Leave this as Bundle not ScriptBundle. Script Bundle minifies JS, MINIFY BREAK ANGULAR.
            bundles.Add(new Bundle("~/Bundles/controllers")
                .Include("~/Scripts/Angular-Setup.js")
                .IncludeDirectory("~/Scripts/Angular-Models/", "*.js", true)
                .IncludeDirectory("~/Scripts/Angular-Components/", "*.js", true)
                .IncludeDirectory("~/Scripts/Angular-Controllers/", "*.js", true));
        }
    }
}
