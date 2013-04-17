using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class StaticContentController : Controller
    {
        //
        // GET: /StaticContent/
        //404 action, doesn't do much
        public ActionResult PageNotFound()
        {
            return View();
        }


    }
}