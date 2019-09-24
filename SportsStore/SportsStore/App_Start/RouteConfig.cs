using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "ProductsCreate",
                url: "Product/Create",
                defaults: new { controller = "Product", action = "Create" }
            );
            routes.MapRoute(
                    name: "ProductsbyCategorybyPage",
                    url: "Product/{category}/Page{page}",
                    defaults: new { controller = "Product", action = "Index" }
);
            routes.MapRoute(
                name: "ProductsbyPage",
                url: "Product/Page{page}",
                defaults: new
                { controller = "Product", action = "Index" }
            );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "ProductsbyCategory",
                url: "Product/{category}",
                defaults: new { controller = "Product", action = "Index" }
            );
            routes.MapRoute(
                name: "ProductIndex",
                url: "Product",
                defaults: new { controller = "Product", action = "Index" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
