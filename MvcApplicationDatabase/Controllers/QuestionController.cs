using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class QuestionController : Controller
    {
        private StackOverflowDbContext db = new StackOverflowDbContext();

        // GET: /Question/
        //
        public ActionResult Index(int page = 1, int pagesize = 15)
        {
            page = page - 1;    // used for paging numbers
            var qList = (from q in db.Questions
                         select q).OrderBy(q => q.DateCreated).Skip(page * pagesize).Take(pagesize);
            ViewBag.PageSize = pagesize; // needed for html paging
            return View(qList);
        }

        public ActionResult Edit(int Question_id)
        {
            var question = from q in db.Questions
                           where q.Id == Question_id
                           select q;

            return View(question);
        }

        public ActionResult Ask()
        {

            return View();
        }
        

    }
}
