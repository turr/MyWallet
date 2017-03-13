using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWallet.MVC5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "MyWallet",
               url: "{lang}/{controller}/{action}",
               defaults: new { lang = "cn", controller = "User", action = "LogOn" }
           );
        }
    }
}
