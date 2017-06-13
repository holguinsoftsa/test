using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ScoutSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Web Api Route needs to come before mvc
            //routes.MapHttpRoute(
            //  "Api_WebApiRoute",
            //  "Api/{controller}/{action}/{id}",
            //  new { action = "Index", id = UrlParameter.Optional }
            //);
            
            //MVC Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
