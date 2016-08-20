using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LOGI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "about"}
            );

            routes.MapRoute(
                name: "KeyIssuePreview",
                url: "KeyIssue/Preview/{id}",
                defaults: new { controller = "KeyIssues", action = "Preview" }
            );

            routes.MapRoute(
                name: "KeyIssue",
                url: "KeyIssue/{id}",
                defaults: new { controller = "KeyIssues", action = "KeyIssue" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
