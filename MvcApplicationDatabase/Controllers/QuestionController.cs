using MvcApplicationDatabase.Models;
using MvcApplicationDatabase.ViewModels;
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
        public ActionResult Index(string sort = "", int page = 1, int pagesize = 15)
        {
            page = page - 1;
            int questionCount = db.Questions.Count();
            IQueryable questionList;
            switch (sort)
            {
                case "faq":
                    questionList = db.Questions.OrderByDescending(q => q.Views)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                default:
                    questionList = db.Questions.OrderByDescending(q => q.DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
            }
            ViewBag.PageSize = pagesize;
            ViewBag.QuestionCount = questionCount;
            ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
            return View(questionList);
        }

        public ActionResult Tagged(string id, int page = 1, int pagesize = 15)
        {
            page = page - 1;
            var questionList = db.Questions.OrderBy(q => q.DateCreated)
                                           .Where(q => q.Tags.Any(w => w.Name == id))
                                           .Skip(page * pagesize)
                                           .Take(pagesize);

            ViewBag.QuestionCount = db.Questions.Count();
            ViewBag.PageSize = pagesize;
            ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
            return View("Index", questionList);
        }


        public ActionResult Ask()
        {

            if ((bool)Session["login"] == true)
            {
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Ask(Question question)
        {
            if (ModelState.IsValid)
            {
                question.DateCreated = DateTime.Now;
                
                db.Posts.Add(new Post()
                    {
                        Content = Request.Form["content"],             
                    });           
                
                // Was unable to add Tags, but fixed this by following the steps under 'Issues With Views': http://oyonti.wordpress.com/2011/05/26/unable-to-update-the-entityset-because-it-has-a-definingquery/
                //
                string[] tagNames = Request.Form["Tag.Name"].Split(' ');
                List<Tag> tagDb = db.Tags.Where(t => tagNames.Contains(t.Name)).ToList(); // Find existing tags in database
                Tag[] tagList = new Tag[tagNames.Count()];

                for (int i=0; i < tagNames.Count(); i++)
                {
                    tagList[i].Name = tagNames[i];
                }

                foreach (Tag tag in tagList)
                {
                    if(!tagDb.Contains(tag))
                        question.Tags.Add(tag);
                }

                /*
                foreach (Tag tag in tagList)
                {
                    question.Tags.Add(tag);
                }
                 * */

                db.Questions.Add(question);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(question);
        }

        public ActionResult Details(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                return View(question);
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(int id)
        {
            var question = db.Questions.Single(q => q.Question_id == id);

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
