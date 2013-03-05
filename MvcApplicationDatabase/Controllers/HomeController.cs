using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class HomeController : Controller
    {
        private StackOverflowDbContext db = new StackOverflowDbContext();

        // GET: /Home/
        //
        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }


    }
}
