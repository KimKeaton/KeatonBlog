using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KeatonBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "NewSlug",
              url: "Blog/Details/{slug}",
              defaults: new { controller = "BlogPosts", action = "Details", slug = UrlParameter.Optional }
          );

            routes.MapRoute(
            name: "Home",
            url: "Home",
            defaults: new { controller = "BlogPosts", action = "Index", id = UrlParameter.Optional }
                 );

            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "BlogPosts", action = "Index", id = UrlParameter.Optional }
);


        }
    }
}
