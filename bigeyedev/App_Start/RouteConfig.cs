using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bigeyedev
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "AddressEdit",
                url: "Home/Account/Address/{id}",
                defaults: new { controller = "Home", action = "AddressEdit", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "AccountEdit",
                url: "Home/Account/Edit",
                defaults: new { controller = "Home", action = "AccountEdit" }
            );


            routes.MapRoute(
               name: "AdminOrder",
               url: "Admin/Order",
               defaults: new { controller = "Admin", action = "ManagerOrder" }
           );

            routes.MapRoute(
               name: "AdminOrderDetail",
               url: "Admin/Order/{id}",
               defaults: new { controller = "Admin", action = "OrderDetial", id = UrlParameter.Optional }
           );


            
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
