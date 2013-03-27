using MvcApplicationDatabase.Models;
using MvcApplicationDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;
using MarkdownSharp;
using System.Web.Services;

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
            IQueryable questions;
            switch (sort)
            {
                case "faq":
                    questions = db.Questions.OrderByDescending(q => q.Views)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                case "reported":
                    questions = db.Questions.OrderByDescending(q => q.Views)
                                           .Where(q => q.Reported != null)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                default:
                    questions = db.Questions.OrderByDescending(q => q.DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
            }
            ViewBag.PageSize = pagesize;
            ViewBag.QuestionCount = questionCount;
            ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
            return View(questions);
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
            if (Session["login"] == null || !(bool)Session["Login"])
                return RedirectToAction("login", "user", new { auth_error = 1 });
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Ask(QuestionFormViewModel vm)
        {
            Markdown md = new Markdown();
            if (ModelState.IsValid)
            {
                vm.Question.DateCreated = DateTime.Now;
                vm.Question.Posts.Add(new Post()
                {
                    Content = md.Transform(Request.Form["Post.Content"]),
                    User_id = (int)Session["ID"],
                    DateCreated = DateTime.Now,
                });         
                
                // Was unable to add Tags, but fixed this by following the steps under 'Issues With Views': http://oyonti.wordpress.com/2011/05/26/unable-to-update-the-entityset-because-it-has-a-definingquery/
                //
                var tagNames = Request.Form["Tag.Name"].Split(" ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var t in tagNames)
                {
                    if (db.Tags.Where(q => q.Name == t).Count() == 0) //More elegant use of extensions goes here
                    {
                        vm.Question.Tags.Add(new Tag()
                        {
                            Name = t,
                            Summary = "Requires summary",
                        });
                    }
                    else
                    {
                        vm.Question.Tags.Add(db.Tags.Where(q => q.Name == t).ToList()[0]);
                    }
                }

                db.Questions.Add(vm.Question);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vm.Question);
        }


        public ActionResult Details(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                var posts = question.Posts.OrderBy(q => q.DateCreated).Skip(1);
                ViewBag.Login = Session["login"];
                ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
                QuestionDetailsFormViewModel model = new QuestionDetailsFormViewModel()
                    {
                        Question = question,
                        BestAnswerPost = question.BestAnswer,
                        OpeningPost = question.Posts.First(),
                        Posts = posts,
                    };

                var row = db.Questions.Where(q => q.Question_id == id).Single();
                row.Views++;
                db.SaveChanges();
                return View(model);
               
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                // Unable to find the question id
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Details(QuestionDetailsFormViewModel model)
        {
            if (Session["login"] == null)
            {
                return RedirectToRoute("Question", new { id = model.Question.Question_id });
            }

            model.NewAnswer.Question_id = model.Question.Question_id;
            model.NewAnswer.DateCreated = DateTime.Now;
            model.NewAnswer.User_id = (int)Session["ID"];

            db.Posts.Add(model.NewAnswer);
            db.SaveChanges();

            return RedirectToAction("Details");
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


        public ActionResult Comment(int id) // id = post_id
        {
            UserController.CheckLogin();

            var comment = new Comment();

            try
            {
                comment.Post_id = id;
                comment.Post = db.Posts.First(p => p.Post_id == id);             
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        [HttpPost]
        public ActionResult Comment(Comment comment)
        {
            UserController.CheckLogin();

            comment.User_id = (int)Session["ID"];
            comment.DateCreated = DateTime.Now;

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToRoute("Question", new { id = comment.Post.Question_id });
        }
        
        [WebMethod()]
        public void Vote(int? id, string type = "up")
        {
            if (id != null)
            {
                var row = db.Posts.Where(p => p.Post_id == id).Single();
                if (type == "up")
                    row.Votes++;
                else if(type == "down")
                    row.Votes--;
                db.SaveChanges();
            }
        }

        public ActionResult Delete(int id = -1)
        {
            bool isAdmin = (Session["username"] != null && db.Users.Any(q => q.Username == Session["username"].ToString()));

            if (isAdmin)
            {
                try
                {
                    db.Questions.First(t => t.Question_id == id).Active = false;
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
