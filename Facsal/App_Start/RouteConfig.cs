﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facsal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Cas",
                url: "Cas/{action}",
                defaults: new { controller = "Cas", action = "Logout" }
            );

            routes.MapRoute(
                name: "ReportFile",
                url: "ReportFile/{action}/{id}",
                defaults: new { controller = "ReportFile", action = "Meeting", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}