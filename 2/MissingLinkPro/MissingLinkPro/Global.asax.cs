using IdentitySample.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /**
         * Redirects away from application if a non-HTTPS connection is detected.
         **/
        //protected void Application_BeginRequest()
        //{
        //    if (!Context.Request.IsSecureConnection)
        //        Response.Redirect("http://www.directagents.com/directlink");
        //}

        /**
         * For testing purposes, use this method to redirect from HTTP to HTTPS.
         * Otherwise, application should use other version.
         **/
        //protected void Application_BeginRequest()
        //{
        //    if (!Context.Request.IsSecureConnection)
        //        Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
        //}
    }
}
