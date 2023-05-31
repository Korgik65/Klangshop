using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Klangshop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Klangshop", action = "Main" }
            );


            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Klangshop", action = "CreateAccount", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CartList",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Klangshop", action = "CartList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ProductList",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Klangshop", action = "ProductList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Product",
                url: "{controller}/{action}/{name}",
                defaults: new { controller = "Klangshop", action = "Product", name = UrlParameter.Optional }
            );

            
        }
    }
}
