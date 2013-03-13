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
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();
        private QuestionController question = new QuestionController();

        // GET: /Home/
        //
        public ActionResult Index()
        {
            var hottopics = db.Questions.OrderByDescending(q => q.DateCreated).Take(5);
            return View(hottopics);
        }
    }
}
