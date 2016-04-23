using System.Web;
using System.Web.Optimization;

namespace gvty.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Assets/js/app").Include(
                        "~/Assets/js/jquery-{version}.js", 
                        "~/Assets/js/foundation.js",
                        "~/Assets/js/motion-ui.js"));
            
        }
    }
}
