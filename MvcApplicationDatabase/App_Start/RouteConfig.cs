using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplicationDatabase
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;


            //
            //  /Question/1234
            //      Maps to:
            // 
            //  /Question/Details/1234
            //
            routes.MapRoute(
                "Question",
                "question/{id}",
                new { controller = "Question", action = "Details" },
                new { id = @"\d+" }
            );

            // 
            //  /Question/1234/answer/submit
            //      Maps to:
            //
            //  [HttpPost]
            //  Question/Details/1234
            //
            routes.MapRoute(
                "AnswerQuestion",
                "Question/{id}/answer/submit",
                new { controller = "Question", action = "Details" },
                new { id = @"\d+", httpMethod = new HttpMethodConstraint("POST") }
            );
            
            // 
            //  /Question/1234/comment/submit
            //      Maps to:
            //
            //  [HttpPost]
            //  Question/Comment/1234
            //
            routes.MapRoute(
                "AddComment",
                "Question/{id}/comment/submit",
                new { controller = "Question", action = "AddComment" },
                new { id = @"\d+", httpMethod = new HttpMethodConstraint("POST") }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}