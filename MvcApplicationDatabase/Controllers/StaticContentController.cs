﻿using System;
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

        public ActionResult PageNotFound()
        {
            return View();
        }

    }
}
