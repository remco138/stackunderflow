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
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();

        // GET: /Question/
        //
        public ActionResult Index(int page = 1, int pagesize = 15)
        {
            page = page - 1;
            var questionList = db.Questions.OrderBy(q => q.DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
            ViewBag.PageSize = pagesize;
            return View(questionList);
        }


        public ActionResult Ask()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ask(Question question, String tagnames)
        {
            // TODO: Moet nog uitzoeken hoe de tags werken

            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(question);
        }


        public ActionResult Edit(int id)
        {
            var question = db.Questions.Single(q => q.Id == id);

            return View(question);
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(question);
        }

        

    }
}
