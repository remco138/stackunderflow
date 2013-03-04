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
        public ActionResult Index()
        {
            var Questions = db.Questions.OrderBy(q => q.DateCreated)
                                        .Take(50); 

            return View();
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
