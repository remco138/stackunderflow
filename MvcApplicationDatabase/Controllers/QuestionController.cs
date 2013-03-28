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
            IQueryable questionList;
            switch (sort)
            {
                case "votes":
                    questionList = db.Questions.OrderByDescending(q => q.Posts.FirstOrDefault().Votes)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                case "active":
                    questionList = db.Questions.OrderByDescending(q => q.Posts.FirstOrDefault().DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                default:
                    /* newest */
                    questionList = db.Questions.OrderByDescending(q => q.DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
            }
            ViewBag.StdPageSize = pagesize;
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


        //  /Question/1234
        //         
        //      Maps to:
        //  
        //  /Question/Details/1234
        //
        public ActionResult Details(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                var posts = question.Posts.OrderBy(q => q.DateCreated).Skip(1);
                ViewBag.Login = Session["login"];

                if (UserController.isLoggedIn)
                    ViewBag.isUserWhoAskedThisQuestion = question.Posts.First().User_id == (int)Session["ID"];
                else
                    ViewBag.isUserWhoAskedThisQuestion = false;

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
        
        //  /Question/1234/answer/submit
        //
        //      Maps to:
        //
        //  [HttpPost]
        //  /Question/Details/1234
        //
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

        //  /Question/10/bestanswer/3
        //
        //      Maps to:
        //  
        //  /Question/SetBestAnswer?question_id=10&bestanswer_id=12
        //
        //      
        public ActionResult SetBestAnswer(int question_id = 0, int bestanswer_id = 0)       
        {
            if (!UserController.isLoggedIn || question_id == 0 || bestanswer_id == 0)
            {
                return RedirectToAction("Index");
            }

            var targetQuestion = db.Questions.First(q => q.Question_id == question_id);
            var targetQuestionUser_id = targetQuestion.Posts.First().User_id;

            // Only the user who created the question can select a best answer.
            //

            if (targetQuestionUser_id == (int)Session["ID"])
            {
                targetQuestion.BestAnswer_id = bestanswer_id;
                db.SaveChanges();

                return RedirectToRoute("Question", new { id = targetQuestion.Question_id });
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                var openingPost = question.Posts.First();

                return View(question);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            bool isAdmin = (Session["username"] != null && db.Users.Any(q => q.Username == Session["username"].ToString()));

            if (ModelState.IsValid && (question.Active == true || isAdmin))
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

            try
            {
                ViewBag.Post_id = id;
                ViewBag.Post = db.Posts.First(p => p.Post_id == id);             
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index");
            }


            return View();
        }

        [HttpPost]
        public ActionResult Comment(Comment comment)
        {
            UserController.CheckLogin();
            bool isAdmin = (Session["username"] != null && db.Users.Any(q => q.Username == Session["username"].ToString()));

            if (comment.Post.Question.Active == false || isAdmin)
            {
                comment.User_id = (int)Session["ID"];
                comment.DateCreated = DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();
            }

            return RedirectToRoute("Question", new { id = comment.Post_id });
        }

        public ActionResult RemoveReport(int id, bool active = true)
        {
            bool isAdmin = (Session["username"] != null && db.Users.Any(q => q.Username == Session["username"].ToString()));

            if (!isAdmin && id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Reported = null;   
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult Active(int id, bool active = true)
        {
            bool isAdmin = (Session["username"] != null && db.Users.Any(q => q.Username == Session["username"].ToString()));

            if (!isAdmin && id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Active = active;
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

    
    public ActionResult Report(int id)
        {
            if (id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Reported = "May contain bad content"; //Introducing a new textbox will be a lot of effort
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }
            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        [WebMethod()]
        public void Vote(int? id, string type = "up")
        {
            if (id != null)
            {
                var row = db.Posts.Where(p => p.Post_id == id).Single();
                if (type == "up")
                    row.Votes++;
                else if (type == "down")
                    row.Votes--;
                db.SaveChanges();
            }
        }
    }
}
