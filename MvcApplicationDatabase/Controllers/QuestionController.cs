using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class QuestionController : Controller
    {
       // private StackOverflowDBContext db = new StackOverflowDBContext();

        //
        // GET: /Question/

        public ActionResult Index()
        {
            ViewBag.Test = "Test";

            return View();
        }
        

    }
}
