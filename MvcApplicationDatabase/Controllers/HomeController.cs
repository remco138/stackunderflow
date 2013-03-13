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
        public ActionResult Index(string sort = "", int page = 1, int pagesize = 15)
        {
            return question.Index(sort, page, pagesize);
        }
    }
}
